using ImaginationWorkgroup.Data.Entities;
using ImaginationWorkgroup.Data.Entities.Audit;
using NHibernate.Event;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Threading;
using System.Threading.Tasks;
using NHibernate;
using NHibernate.Persister.Entity;

namespace ImaginationWorkgroup.Data.Configuration
{
    public class NhEventListener : IPreUpdateEventListener, IPreInsertEventListener, IPostUpdateEventListener
    {

        private static readonly string CreatedProp = EntityPersisterUtility.GetPropertyName<ImaginationEntityBase>(val => val.Created);
        private static readonly string ModifiedProp = EntityPersisterUtility.GetPropertyName<ImaginationEntityBase>(val => val.Modified);
        private static readonly string ModifiedByProp = EntityPersisterUtility.GetPropertyName<ModifiedByEntityBase>(val => val.ModifiedBy);
        public void OnPostUpdate(PostUpdateEvent @event)
        {
            //if we somehow update somethingn that is not entity base type, just ignore it
            if (@event.Entity is ImaginationEntityBase entityBase)
            {
                Idea associatedIdea;
                if (@event.Entity is Idea)
                {
                    associatedIdea = @event.Entity as Idea;
                }
                else
                {
                    associatedIdea = GetParentIdea(@event.Entity);
                }

                var employee = GetEmployeeForRequest(@event.Session);

                var session = @event.Session.SessionWithOptions().OpenSession();

                var hist = new History(@event.Entity.GetType().Name, entityBase.Id, associatedIdea, employee);
                session.Save(hist);

                var dirtyIndices = @event.Persister.FindDirty(@event.State, @event.OldState, @event.Entity, @event.Session);
                foreach(var index in dirtyIndices)
                {
                    var oldVal = EntityPersisterUtility.GetPropertyValue(@event.OldState, index);
                    var newVal = EntityPersisterUtility.GetPropertyValue(@event.State, index);
                    var propName = @event.Persister.PropertyNames[index];
                    var histItem = new HistoryItem(hist, propName, oldVal, newVal);
                    session.Save(histItem);
                }
            }
        }

        public Task OnPostUpdateAsync(PostUpdateEvent @event, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            OnPostUpdate(@event);
            return Task.CompletedTask;
        }

        public bool OnPreInsert(PreInsertEvent @event)
        {
            if (@event.Entity is INoAuditTrigger)
                return false;
            SetBaseTimestamps(@event.Entity, @event.Persister, @event.State, true);
            TrySetModifiedBy(@event.Entity, @event.Persister, @event.Session, @event.State);
            return false;
        }

        public Task<bool> OnPreInsertAsync(PreInsertEvent @event, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var result = OnPreInsert(@event);
            return Task.FromResult(result);

        }

        public bool OnPreUpdate(PreUpdateEvent @event)
        {
            if (@event.Entity is INoAuditTrigger)
                return false;
            SetBaseTimestamps(@event.Entity, @event.Persister, @event.State, false);
            TrySetModifiedBy(@event.Entity, @event.Persister, @event.Session, @event.State);
            return false;
        }

        public Task<bool> OnPreUpdateAsync(PreUpdateEvent @event, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var result = OnPreUpdate(@event);
            return Task.FromResult(result);
        }

        private Idea GetParentIdea(object entity)
        {
            var entityType = entity.GetType();
            var ideaProp = entityType.GetProperties()
                .Where(typ => typ.PropertyType == typeof(Idea)
                        && entityType != typeof(Idea))  //Idea doesn't have a parent/child idea right now, but we should avoid infinite loops if that changes
                .FirstOrDefault();
            if (ideaProp != null)
            {
                var idea = (Idea)ideaProp.GetValue(entity);
                return idea;
            }
            else
            {
                return null;
            }
        }

        private void UpdateIdeaModifiedData(Idea idea, EmployeeProfile modifiedBy, ISession session)
        {
            var theIdea = session.Get<Idea>(idea.Id);
            theIdea.Modified = DateTimeOffset.Now;
            theIdea.ModifiedBy = modifiedBy;
            session.Update(theIdea);
            session.Flush();
        }

        private EmployeeProfile GetEmployeeForRequest(ISession session)
        {
            if (HttpContext.Current != null)
            {
                var currentUser = HttpContext.Current.User.Identity.Name;
                var pin = currentUser.Split('\\').Last();
                return session.Query<EmployeeProfile>().OrderByDescending(ep => ep.Created).FirstOrDefault(ep => ep.UserPin == pin);
            }
            return null;
        }

        private void SetBaseTimestamps(object entity, IEntityPersister persister, object[] state, bool setCreate)
        {
            if (entity is ImaginationEntityBase entityBase)
            {
                var dt = DateTimeOffset.Now;
                entityBase.Modified = dt;
                EntityPersisterUtility.SetState(persister, state, ModifiedProp, dt);

                if (setCreate)
                {
                    entityBase.Created = dt;
                    EntityPersisterUtility.SetState(persister, state, CreatedProp, dt);
                }
            }
        }

        private void TrySetModifiedBy(object entity, IEntityPersister persister, ISession session, object[] state)
        {
            var employee = GetEmployeeForRequest(session);
            if (entity is ModifiedByEntityBase modifiedByBase)
            {
                modifiedByBase.ModifiedBy = employee;
                EntityPersisterUtility.SetState(persister, state, ModifiedByProp, employee);
            }

            var idea = GetParentIdea(entity);
            if (idea != null)
            {
                UpdateIdeaModifiedData(idea, employee, session.SessionWithOptions().OpenSession());
            }
        }
    }
}
