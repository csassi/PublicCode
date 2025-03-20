;

var iwg = iwg || {};


iwg.usageHawk = {
    save: function (additionalData) {
        $.ajax({
            url: '/api/Usage',
            type: 'post',
            data: {
                Pathname: window.location.pathname,
                Query: window.location.search,
                AdditionalData: JSON.stringify(additionalData)
            },
            dataType: 'json',
            error: function (xhr, error, status) {
                console.log('unable to record usage');
                console.log(xhr);
            }
        })
    }
};

$(window.document).ready(function () {
    iwg.usageHawk.save();
})