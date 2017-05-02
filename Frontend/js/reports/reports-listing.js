var TableDatatablesResponsive = function () {

    let initListingTable = function (reportListingData) {
        let table = $('#report_listing');
        let oTable = table.dataTable({
            data : reportListingData,
            // Internationalisation. For more info refer to http://datatables.net/manual/i18n
            "language": {
                "aria": {
                    "sortAscending": ": activate to sort column ascending",
                    "sortDescending": ": activate to sort column descending"
                },
                "emptyTable": "No data available in table",
                "info": "Showing _START_ to _END_ of _TOTAL_ entries",
                "infoEmpty": "No entries found",
                "infoFiltered": "(filtered1 from _MAX_ total entries)",
                "lengthMenu": "_MENU_ entries",
                "search": "Search:",
                "zeroRecords": "No matching records found"
            },
            bDestroy: true,
            bFilter: false, //hide Search bar
            bPaginate: false,//hide pagination
            bInfo: false, // hide showing entries
            bSortable: false, //disable sorting
            bLengthChange: false,

            columns: [
                { "data": "Description" },
                { "data": "SurveyID", visible: false },
                {
                    "className": 'details-control',
                    "orderable": false,
                    "data": null,
                    "defaultContent": '<button type="button" class="btn dark btn-outline report"><i class="fa fa-line-chart"></i> View Report</button>' +
                        '<button type="button" class="btn purple btn-outline upper view-data"><i class="fa fa-file-text"></i> View Data</button>' +
						'<button type="button" class="btn green btn-outline upper direct-input"><i class="fa fa-file-text"></i> Direct Input</button>' +
						'<button type="button" class="btn blue btn-outline upper comments-input"><i class="fa fa-file-text"></i> Comments Input</button>'
                }
            ],
            "order": [
                [0, 'asc']
            ],
            "lengthMenu": [
                [5, 10, 15, 20, -1],
                [5, 10, 15, 20, "All"] // change per page values here
            ],
            // set the initial value
            "pageLength": 10,
            "dom": "<'row' ><'row'<'col-md-4 col-sm-4'l><'col-md-8 col-sm-8'f>r><'table-scrollable't><'row'<'col-md-4 col-sm-4'i><'col-md-8 col-sm-8'p>>", // horizobtal scrollable datatable
        });
    }

    return {
        //main function to initiate the module
        init: function (reportListingData) {
            if (!jQuery().dataTable)
                return;
            initListingTable(reportListingData);
        }
    };

}();

jQuery(document).ready(function () {
    
    //check if authenticated    
    AccessControl.init();

    let passcode = Utils.getPasscode();
    let url = Config.getApiRootUrl() + '/reports/surveylist/' + passcode;
    
    $.ajax({
        url: url,
        beforeSend: function () {
            Utils.blockUI({
                target: '#report-listing-div',
                message: 'Loading...', 
                boxed : true
            });
        },
        complete: function () {
            Utils.unblockUI('#report-listing-div');
        },
        success: function (json) {
            $('#survey-name-span').text(json[0].SurveyName);
            TableDatatablesResponsive.init(json);
        }
    });    

    $('#report_listing tbody').on('click', 'button', function () {
        var data = $('#report_listing').DataTable().row($(this).parents('tr')).data();
        //Store 
        localStorage.setItem("halo_current_surveyid", data.SurveyID);
        if ($(this).hasClass('report'))
        {
            window.location.href = 'report.html?siteid=' + data.SurveyID;
        }
        if ($(this).hasClass('view-data')) {
            window.location.href = 'view_data.html?siteid=' + data.SurveyID;
        }
        if ($(this).hasClass('direct-input')) {
            window.location.href = Config.getDirectInputUrl() + '?siteid=' + data.SurveyID;
        }
        if ($(this).hasClass('comments-input')) {
            window.location.href = 'comments-input.html?siteid=' + data.SurveyID;
        }
        
    });
});



