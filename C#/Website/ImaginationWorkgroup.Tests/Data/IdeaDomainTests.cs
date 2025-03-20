using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ImaginationWorkgroup.Data.Repositories;
using ImaginationWorkgroup.Data.Domains;
using ImaginationWorkgroup.Data.Entities;
using System.Linq;
using ImaginationWorkgroup.Data.Dto;
using ImaginationWorkgroup.Infrastructure.Services;
using ImaginationWorkgroup.Infrastructure.QueryHelpers;

namespace ImaginationWorkgroup.Tests.Data
{
    /// <summary>
    /// Summary description for IdeaDomainTests
    /// </summary>
    [TestClass]
    public class IdeaDomainTests
    {
        private Mock<IRepository> _repo;
        private IIdeaDomain _ideaDomain;
        private IIdeaService _ideaService;
        public IdeaDomainTests()
        {
            _repo = new Mock<IRepository>();

            _ideaDomain = new IdeaDomain(_repo.Object);
            _ideaService = new IdeaService(_ideaDomain, new Mock<IIdeaFilterAppender>().Object, new Mock<IReviewerService>().Object);
        }

        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GetMapThrowsNullEx()
        {
            Idea idea = null;
            _ideaDomain.GetMapsForIdea(idea);
        }

        [TestMethod]
        public void RouteIdeaSavesStatusChangeOnApproval()
        {
            var currentStatus = new IdeaStatus();
            var expectedStatus = new IdeaStatus();
            var map = new StatusMap()
            {
                Id = 200,
                NextMap = new MapAttribute { Status = expectedStatus },
                CurrentStatus = currentStatus
            };
            _repo.Setup(r => r.Get<StatusMap>(It.IsAny<object>())).Returns(map);

            var idea = new Idea() { CurrentStatus = currentStatus };
            _ideaService.RouteIdea(idea, new EmployeeProfile(), 200, "Test comment");
            _repo.Verify(e => e.Update(It.Is<Idea>(i => i.CurrentStatus == expectedStatus)), Times.Once());
        }

    }
}
