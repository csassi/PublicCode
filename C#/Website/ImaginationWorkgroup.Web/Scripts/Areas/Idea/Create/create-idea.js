;
var create = create || {};

var patterns = {
    email: /^([\d\w-\.]+@(gmail\.)com)?$/i,
    phone: /^\d[\d -]*\d$/,
    postcode: /^([a-zA-Z]{1,2}[0-9][0-9]?[a-zA-Z\s]?\s*[0-9][a-zA-Z]{2,2})|(GIR 0AA)$/
};

create.CreateIdeaModel = function () {

    var self = this;
    self.Model = null;

    self.componentItems = ko.observableArray([]);
    self.locationItems = ko.observableArray([]);

    self.IdeaTitle = ko.observable('').extend({
        required: {
            message: 'Please enter your idea title.'
        }
    });
    self.IdeaProblem = ko.observable('').extend({
        required: {
            message: 'Please enter your problem.'
        }
    });
    self.IdeaText = ko.observable('').extend({
        required: {
            message: 'Please enter your idea.'
        }
    });
    self.IdeaBenefits = ko.observable('').extend({
        required: {
            message: 'Please enter your idea benefits.'
        }
    }); 
    self.SelectedLocation = ko.observable().extend({
        required: {
            message: 'Please enter your location.'
        }
    });
    self.SelectedComponent = ko.observable().extend({
        required: {
            message: 'Please enter your component.'
        }
    });
    self.SupervisorEmail = ko.observable("").extend({
        required: {
            params: true,
            message: 'Please enter your supervisor\'s email.'
        },
        pattern: {
            message: 'Please enter a valid gmail.com email.',
            params: patterns.email
        }
    });
    self.checkEmail = function (param1, param2) {
        self.SupervisorEmail(param2.item.value);
        self.CheckEmailValid();
    };

    self.SearchText = ko.observable();

    self.MaxTitleChars = 100;
    self.MaxProblemChars = 1500;
    self.MaxIdeaChars = 1500;
    self.MaxBenefitChars = 1500;

    self.TitlePlaceholder = ko.observable();
    self.ProblemPlaceholder = ko.observable();
    self.BenefitPlaceholder = ko.observable();
    self.IdeaPlaceholder = ko.observable();

    self.TitleChars = ko.computed(function () {
        if (self.IdeaTitle() === '')
            return 0;
        return self.IdeaTitle().length;
    });
    self.ProblemChars = ko.computed(function () {
        if (self.IdeaProblem() === '')
            return 0;
        return self.IdeaProblem().length;
    });
    self.IdeaChars = ko.computed(function () {
        if (self.IdeaText() === '')
            return 0;
        return self.IdeaText().length;
    });
    self.BenefitChars = ko.computed(function () {
        if (self.IdeaBenefits() === '')
            return 0;
        return self.IdeaBenefits().length;
    });


    self.TitleIsMaxLength = ko.computed(function () {
        if (self.TitleChars() !== self.MaxTitleChars) {
            return false;
        }
        else {
            return true;
        }
    });
    self.ProblemIsMaxLength = ko.computed(function () {
        if (self.ProblemChars() !== self.MaxProblemChars) {
            return false;
        }
        else {
            return true;
        }
    });
    self.IdeaIsMaxLength = ko.computed(function () {
        if (self.IdeaChars() !== self.MaxIdeaChars) {
            return false;
        }
        else {
            return true;
        }
    });
    self.BenefitIsMaxLength = ko.computed(function () {
        if (self.BenefitChars() !== self.MaxBenefitChars) {
            return false;
        }
        else {
            return true;
        }
    });

    self.IsEmailValid = ko.observable(false);
    self.CheckEmailValid = function () {

        self.SubmitEnabled(false);
        self.SubmitText("...Checking email");
        $.ajax({
            url: '/api/UserInfo/ByEmail',
            type: 'get',
            dataType: 'json',
            data: { email: self.SupervisorEmail() },

            success: function (data) {
                self.IsEmailValid(true);
            },
            error: function (xhr, options, error) {
                console.log(xhr);
                self.IsEmailValid(false);
            },
            complete: function () {

                self.SubmitEnabled(true);
                self.SubmitText("Submit Idea");

                if (self.IsEmailValid()) {
                    self.ShowThumbsUp(true);
                } else {
                    self.SupervisorEmail.setError("Please enter a valid gmail.com email.");
                    self.ShowThumbsUp(false);
                }
            }
        });
    };

    self.ShowThumbsUp = ko.observable(false);
    self.CheckThumbsUp = ko.computed(function () {
        if (self.SupervisorEmail().length === 0) {
            return false;
        }
        return self.ShowThumbsUp();
    });
    self.SupervisorEmail.subscribe(function (newValue) {

        var theEmail = self.SupervisorEmail();
        var patt = new RegExp(patterns.email);
        var res = patt.test(theEmail);
        if (res) {
            self.CheckEmailValid();
        }
        else {
            self.ShowThumbsUp(false);
        }
    });

    self.searchBlurred = function () {
        //Do stuff
    };
    self.getAutocompleteValues = function (request, response) {

        if (/\d/.test(request.term)) {
            $.ajax({
                url: '/api/UserInfo/ByPin',
                type: 'get',
                dataType: 'json',
                data: { pin: request.term },

                success: function (data) {
                    var items = [
                        { label: data.Employee.Email, value: data.Employee.Email, userData: data.Employee }
                    ];
                    response(items);
                },
                error: function (xhr, options, error) {
                    console.log(xhr);

                }
            });
        } else {
            $.ajax({
                url: '/api/UserInfo/ByName',
                type: 'get',
                dataType: 'json',
                data: { name: request.term },
                success: function (data) {
                    var items = $.map(data.Employees, function (item) {
                        return { label: item.Email, value: item.Email, userData: item };
                    });
                    response(items);
                },
                error: function (xhr, options, error) {
                    console.log(xhr);
                }
            });
        }

    };

    self.TitleCharCounter = ko.computed(function () {
        if (!self.TitleIsMaxLength()) {
            return self.TitleChars() + "/" + self.MaxTitleChars;
        }
        else {
            return "Character limit has been reached";
        }
    });

    self.IdeaCharCounter = ko.computed(function () {
        if (!self.IdeaIsMaxLength()) {
            return self.IdeaChars() + "/" + self.MaxIdeaChars;
        }
        else {
            return "Character limit has been reached";
        }
    });

    self.ProblemCharCounter = ko.computed(function () {
        if (!self.ProblemIsMaxLength()) {
            return self.ProblemChars() + "/" + self.MaxProblemChars;
        }
        else {
            return "Character limit has been reached";
        }
    });
    self.BenefitCharCounter = ko.computed(function () {
        if (!self.BenefitIsMaxLength()) {
            return self.BenefitChars() + "/" + self.MaxBenefitChars;
        }
        else {
            return "Character limit has been reached";
        }
    });

    self.SubmitText = ko.observable('Submit Idea');
    self.SubmitEnabled = ko.observable(true);

    self.submitIdea = function () {

        if (self.errors().length === 0) {
            self.SubmitEnabled(false);
            self.SubmitText('...submitting');
            $('#SubmitIdeaForm').submit();
            return false; //why does chrome not post??!! I need to call the form post manually....
        }
        else {
            self.CloseModal();
            self.errors.showAllMessages();
            //Show focus to the top most textbox.
            var theElement = $(".input-validation-error").first()[0];
            theElement.scrollIntoView(false);
            theElement.focus();
           
            return false;
        }

    };

    self.ShowModal = function () {

        $('#SubmitIdeaModel').modal('show');
    };
    self.CloseModal = function () {

        $('#SubmitIdeaModel').modal('hide');

        self.SubmitEnabled(true);
        self.SubmitText('Submit Idea');
    };

};

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
        errorMessageClass: "alert alert-danger alert-red alert-body validation-message",
        errorElementClass: 'input-validation-error'
    }, true);

    var viewmodel = new create.CreateIdeaModel();

    viewmodel.errors = ko.validation.group(viewmodel);
    viewmodel.Model = CreateIdeaModel;
    viewmodel.componentItems = CreateIdeaModel.Components;
    viewmodel.locationItems = CreateIdeaModel.Locations;

    viewmodel.TitlePlaceholder = CreateIdeaModel.IdeaTitlePlaceholder;
    viewmodel.ProblemPlaceholder = CreateIdeaModel.IdeaProblemPlaceholder;
    viewmodel.BenefitPlaceholder = CreateIdeaModel.IdeaBenefitsPlaceholder;
    viewmodel.IdeaPlaceholder = CreateIdeaModel.IdeaTextPlaceholder;

    ko.applyBindings(viewmodel, document.body);
});