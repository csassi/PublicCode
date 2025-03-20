;

var iwg = iwg || {};


$(window.document).ready(function () {
    var listView = new iwg.ideaListView();
    ko.applyBindings(listView, document.body);

    listView.init();
    $('.tablesorter').tablesorter({ sortList: [[0, 1]] });

})


iwg.ideaListView = function (data) {
    var self = this;
    self.listItems = ko.observableArray([]);
    self.isReviewer = ko.observable(false);
    self.filterItems = ko.observableArray([]);
    self.viewIdeaDetails = ko.observable(false);
    self.loading = false;
    self.currentFilter = ko.observable('');

    $('#loading-modal').on('shown.bs.modal', function (e) {
        if (!self.loading) {
            $('#loading-modal').modal('hide');
        }
    });


    self.shouldShowIdea = function (listItem) {
        //very specific scenario for should be hidden for employees, user is reviewer
        return (listItem.displayForEmployee() || listItem.canAccess()) && self.viewIdeaDetails();
    }

    self.getFilterTypes = function () {
        $.ajax({
            url: '/api/IdeaFilter',
            type: 'get',
            dataType: 'json',
            success: function (data) {
                $.map(data.Model, function (filter) { self.filterItems.push(new iwg.filterItem(filter)); });
                if (self.filterItems().length > 0) {
                    var firstFilter = self.filterItems()[0];
                    self.updateSelectedFilter(firstFilter);
                }
            },
            error: function (xhr, error, status) {
                console.log(xhr);
            }
        })
    }

    self.getListItems = function (filterType) {
        $.ajax({
            url: '/api/IdeaListing',
            type: 'get',
            data: { filter: filterType },
            dataType: 'json',
            success: function (data) {
                self.isReviewer(data.Model.IsReviewer);
                self.viewIdeaDetails(data.Model.ViewIdeaDetails);
                self.listItems([]);
                $('.tablesorter').find('tbody').empty();
                $('.tablesorter').trigger('update');
                //self.listItems.push($.map(data.Model, function (model) { return new iwg.ideaListItem(model) }));
                $.map(data.Model.ListItems, function (model) { self.listItems.push(new iwg.ideaListItem(model)); });
                $(".tablesorter").trigger("update");
            },
            error: function (xhr, error, status) {
                alert('an error occurred while fetching the data');
                console.log(xhr);
            },
            complete: function () {
                self.loading = false;
                $('#loading-modal').modal('hide');
            }
        });
    }



    self.updateSelectedFilter = function (filter) {
        if (self.loading)
            return;
        self.loading = true;
        self.filterItems().forEach(function (fi) { fi.active(false); });
        self.currentFilter(filter.display());
        $('#loading-modal').modal('show');
        filter.active(true);
        iwg.usageHawk.save({ Filter: filter.display() });
        self.getListItems(filter.filterType());
    }

    self.init = function () {
        self.getFilterTypes();
    }
}

iwg.filterItem = function (data) {
    var self = this;

    self.active = ko.observable(data.Active);
    self.display = ko.observable(data.DisplayText);
    self.title = ko.observable(data.Title);
    self.filterType = ko.observable(data.FilterType);

}

iwg.ideaListItem = function (data) {
    var self = this;

    self.id = ko.observable(data.Id);
    self.title = ko.observable(data.Title);
    self.status = ko.observable(data.Status);
    self.created = ko.observable(data.CreatedFormatted);
    self.modified = ko.observable(data.ModifiedFormatted);
    self.creator = ko.observable(data.Creator);
    self.component = ko.observable(data.Component);
    self.createdLocal = ko.observable(data.CreatedLocal);
    self.modifiedLocal = ko.observable(data.ModifiedLocal);
    self.displayForEmployee = ko.observable(data.DisplayForEmployee);
    self.location = ko.observable(data.Location);
    self.canAccess = ko.observable(data.CanAccess);

    self.titleDisplay = ko.computed(function () {
        if (self.displayForEmployee())
            return self.id().toString() + '-' + self.title();
        else
            return self.id().toString() + '-' + 'Unmoderated';
    });

    self.detailsLink = ko.computed(function () {
        return '/Idea/Details/Index/' + self.id().toString();
    });

}