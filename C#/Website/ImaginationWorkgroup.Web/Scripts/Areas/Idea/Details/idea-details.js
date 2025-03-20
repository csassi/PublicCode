;
var details = details || {};


details.IdeaDetailsModel = function () {
    var self = this;
    var states = {
        ready: 'ready',
        promptOpen: 'promptOpen',
        promptSubmitting: 'posting'
    };

    self.state = ko.observable(states.ready);

    self.Model = null;

    self.commentItems = ko.observableArray([]);
    self.statusItems = ko.observableArray([]);
    self.statusMaps = ko.observableArray([]);

    self.selectedWorkLocation = ko.observable();

    self.selectedLocationText = ko.computed(function () {
        var locationId = self.selectedWorkLocation();
        let locationText = '';
        if (self.Model != null) {
            self.Model.LocationChangeModel.PossibleLocations.forEach(function (val) {
                if (val.Id == locationId) {
                    locationText = val.Location;
                }
            });
        }
        return locationText;
    });

    self.canReviewIdea = ko.observable(false);

    self.displayReviewPanel = ko.computed(function () {
        return self.canReviewIdea() && self.statusMaps().length > 0;
    });

    self.displayLocationPanel = ko.computed(function () {
        return self.canReviewIdea() && self.Model.LocationChangeModel.CanChangeLocation;
    });

    self.statusMapsEnabled = ko.computed(function () {
        return self.state() === states.ready;
    });

    //selectedStatusMap is used for the confirmation modal dialog. 
    self.selectedStatusMap = ko.observable();
    self.statusMapSelected = function (statusMap) {
        self.selectedStatusMap(statusMap);
        $('#ProcessFlowModal').modal('show');
    }


    self.Comment = ko.observable('').extend({
        required: {
            message: 'Please enter your comment.'
        }
    });

    self.raiseLocationModal = function () {
        $('#location-change-modal').modal('show');
        
    };

    self.postLocationChange = function () {
        self.state(states.promptSubmitting)
        $.ajax({
            url: '/api/Idea/' + self.Model.Id.toString(),
            data: {
                WorkLocationId: self.selectedWorkLocation()
            },
            type: 'PATCH',
            success: function () {
                window.location.href = '/Idea/Details/LocationChanged/' + self.Model.Id.toString();
            },
            error: function (xhr, error, status) {
                console.log(xhr, error, status);
                alert('An error occurred while submitting the location change. Please refresh the page and try again.');
            }

        })
    };

    self.canChangeLocation = ko.computed(function () {
        return self.state() != states.promptSubmitting;
    });

    self.MaxCommentChars = 1000;
    self.MaxModalCommentChars = 1000;
    self.ModalCommentPlaceHolderText = ko.observable("Please enter the reason that you are not approving this idea. This reason will be emailed to others, including the employee.");

    self.CommentChars = ko.computed(function () {
        if (self.Comment() === '' || self.Comment() === null || self.Comment() === undefined)
            return 0;
        return self.Comment().length;
    });
    self.CommentIsMaxLength = ko.computed(function () {
        if (self.CommentChars() !== self.MaxCommentChars) {
            return false;
        }
        else {
            return true;
        }
    });
    self.CommentCharCounter = ko.computed(function () {
        if (!self.CommentIsMaxLength()) {
            return self.CommentChars() + "/" + self.MaxCommentChars;
        }
        else {
            return "Character limit has been reached";
        }
    });

    self.SubmitText = ko.observable('Add Comment');
    self.SubmitEnabled = ko.observable(true);

    self.submitComment = function () {

        var theIdeaId = self.Model.Id;
        if (self.errors().length === 0) {
            self.SubmitEnabled(false);
            self.SubmitText('...submitting');
            self.addComment(theIdeaId);
        }
        else {
            self.errors.showAllMessages();
            return false;
        }

    };

    self.modalButtonText = ko.computed(function () {
        if (self.state() === states.promptSubmitting) {
            return '...submitting';
        }
        else {
            return self.selectedStatusMap() == null ? 'Submit' : self.selectedStatusMap().DisplayText;
        }
    })
    self.SubmitApproveTitle = ko.observable('');
    self.SubmitApproveText = ko.observable('');
    self.SubmitApproveEnabled = ko.observable(true);
    self.SubmitRejectEnabled = ko.observable(true);
    self.modalSubmitClick = function () {
        self.state(states.promptSubmitting);
        $.ajax({
            url: '/api/IdeaReview',
            type: 'POST',
            data: { IdeaId: self.Model.Id, Comment: self.ModalComment(), SelectedMapId: self.selectedStatusMap().Id },
            dataType: 'json',
            success: function (data) {
                console.log("Submited idea review successfully!");
                window.location.href = "/Idea/Details/ReviewComplete/" + self.Model.Id;
            },
            error: function (xhr, error, status) {
                console.log("Failed to submit review!");
                console.log(xhr, error, status);
                alert('There was an error routing this idea. Please refresh the page and try again. If you continue to experience issues, please report this error to the Help Desk');
            },
        });

    }


    self.SubmitRejectTitle = ko.observable('');
    self.SubmitRejectText = ko.observable('');


    self.ModalCloseButtonText = ko.observable('Close');
    self.ModalText = ko.observable('');
    self.ModalComment = ko.observable('');
    self.ModalCommentChars = ko.computed(function () {
        if (self.ModalComment() === '')
            return 0;
        return self.ModalComment().length;
    });
    self.ModalCommentIsMaxLength = ko.computed(function () {
        if (self.ModalCommentChars() !== self.MaxModalCommentChars) {
            return false;
        }
        else {
            return true;
        }
    });
    self.ModalCommentCharCounter = ko.computed(function () {
        if (!self.ModalCommentIsMaxLength()) {
            return self.ModalCommentChars() + "/" + self.MaxModalCommentChars;
        }
        else {
            return "Character limit has been reached";
        }
    });


    self.canSubmitModal = ko.computed(function () {
        if (self.state() === states.promptSubmitting)
            return false;
        else {
            var required;

            return self.ModalComment().length > 0 || !(self.selectedStatusMap() == null ? true : self.selectedStatusMap().RequireComments);
        }

        return self.ModalComment()
    });

    self.CloseModal = function () {

        $('#SubmitIdeaModel').modal('hide');

        self.SubmitEnabled(true);
        self.SubmitText('Submit Idea');
    };

    self.getComments = function () {

        self.commentItems.removeAll();

        $.ajax({
            url: '/api/Comment',
            type: 'GET',
            data: { id: self.Model.Id },
            dataType: 'json',
            success: function (data) {
                $.map(data.Model, function (model) { self.commentItems.push(model); });

            },
            error: function (xhr, error, status) {
                console.log(xhr);
            },
            complete: function () {

            }
        });
    };

    self.addComment = function (id) {

        $.ajax({
            url: '/api/Comment',
            type: 'POST',
            data: { id: id, param: self.Comment() },
            dataType: 'json',
            success: function (data) {
                console.log("Added comment successfully!");
                self.Comment(undefined);    //this resets the field.
                self.Comment.clearError();  //this resets the validation.
            },
            error: function (xhr, error, status) {
                console.log("Failed to add comment!");
                console.log(xhr);
            },
            complete: function () {

                self.commentItems.removeAll();
                self.getComments();
                self.SubmitEnabled(true);
                self.SubmitText('Add Comment');
            }
        });
    };

};

details.statusMapModel = function (data) {

}

$(window.document).ready(function () {

    ko.validation.init({
        registerExtenders: true,
        messagesOnModified: true,
        insertMessages: true,
        parseInputAttributes: true,
        messageTemplate: null,
        errorsAsTitle: true,
        decorateElementOnModified: true,
        decorateInputElement: true,
        errorMessageClass: "alert alert-danger alert-red alert-body validation-message"
    }, true);

    var viewmodel = new details.IdeaDetailsModel();
    viewmodel.Model = IdeaDetailsModel;
    viewmodel.canReviewIdea(IdeaDetailsModel.CanReviewIdea);
    viewmodel.statusMaps(IdeaDetailsModel.StatusMaps);

    viewmodel.SubmitEnabled(IdeaDetailsModel.RenderCommentButton);
    viewmodel.statusItems = IdeaDetailsModel.StatusChanges;
    viewmodel.errors = ko.validation.group(viewmodel);
    ko.applyBindings(viewmodel, document.body);
    viewmodel.getComments();

});
