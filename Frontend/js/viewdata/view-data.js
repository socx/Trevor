
var HaloViewData = function () {
    
    $.extend($.fn.dataTable.defaults, {
        responsive: true
    });

    var buttonCommon = {
        exportOptions: {
            format: {
                body: function (data, column, row) {
                    if (column === 2) {
                        if (data.toLowerCase().indexOf("flag_red") !== -1) {
                            return 'True';
                        }
                    }
                    return data;
                }
            }
        }
    };
    var getTooltip = function (columns, key) {
        var toolTip = '';
        $.each(columns, function (i, column) {
            if (key == column.title)
            {
                toolTip = column.tooltip;
            }
        });
        return toolTip;
    }

    return {   
        init : function (){
            if (typeof (Storage) !== "undefined") {
                let surveyID = localStorage.getItem("halo_current_surveyid");
                if (surveyID && !isNaN(surveyID) && (surveyID > 0)) {
                    $.ajax({
                        url: Config.getApiRootUrl() + '/reports/surveys/' + surveyID,
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
                        success: function (result) {
                            $('#header-span').text(result.SurveyName + ' Responses');
                        }
                    });
                }
                else {
                    window.location.href = 'client-login.html'; //send back to login page
                }
            }

        },
        initDateRange: function (){
            $('#from').val(moment().subtract(1, 'day').subtract(1, 'month').format("DD/MM/YYYY"));
            $('#to').val(moment().subtract(1, 'day').format("DD/MM/YYYY"));    
        },
        buildRequestData : function (surveyID){
            let dateRange = Utils.getApiDateRange($('#from').val(), $('#to').val());
            let startDate = dateRange.StartDate;
            let endDate = dateRange.EndDate;
            let imagePath = Config.getAppRootUrl() + "/images";

            let parameters = 'SurveyID=' + surveyID 
            parameters += startDate ? '&StartDate=' + startDate : '' 
            parameters += endDate ? '&EndDate=' + endDate : '';
            parameters += imagePath ? '&ImagePath=' + imagePath : '';
            return parameters;

        },
        getViewData: function (surveyID, requestData) {
            if (requestData) {
                $.ajax({
                    url: Config.getApiRootUrl() + '/viewdata/' + surveyID + '?' + requestData,
                    type: "GET",
                    dataType: "json",
                    contentType: "application/json; charset=utf-8",
                    data: requestData,
                    beforeSend: function () {
                        Utils.blockUI({
                            target: '#view_data',
                            message: 'Loading...', 
                            boxed : true
                        });
                    },
                    complete: function () {
                        Utils.unblockUI('#view_data');
                    },
                    success: function (json) {
                        HaloViewData.drawViewDataTable(json);
                    }
                });
            }
        },
        drawViewDataTable: function (json) {
            var tableHeaders = '';
            if ((json.Columns != null) && (json.Columns.length > 0) && (json.Data != null) && (json.Data.length > 0)) {
                var exportColumns = [];
                $.each(json.Columns, function (i, val) {
                    tableHeaders += "<th class='all'>" + val.title + "</th>";
                    if ((val.title != "Red Flag") && (val.title != "Amber Flag")) {
                        exportColumns.push(i);
                    }
                });

                $('#message-div').addClass('display-hide');
                $('#responses-div').removeClass('display-hide');

                $("#view_data").empty();
                $("#view_data").append('<table id="surveyDataTable" class="table table-striped table-bordered table-compressed table-hover dt-responsive" cellspacing="0" width="100%"><thead><tr>' + tableHeaders + '</tr></thead></table>');
                var table = $('#surveyDataTable').dataTable({
                    bDestroy: true,
                    data: json.Data,
                    columns: json.Columns,
                    bFilter: false, //hide Search bar
                    bInfo: true,
                    bSortable: false, //disable sorting
                    lengthMenu: [[25, 50, 100, -1], [25, 50, 100, "All"]],
                    language: {
                        lengthMenu: "Show _MENU_ records per page",
                    },
                    initComplete: function (settings) {
                        var colIndex = 0;
                        $('#surveyDataTable thead th').each(function () {
                            var $td = $(this);
                            
                            // After flag columns add 2 to skip the flag image columns
                            if (colIndex == 3)
                            {
                                colIndex = colIndex + 2;
                            }
                            var tooltip = json.Columns[colIndex].tooltip;
                            $td.attr('title', tooltip);
                            colIndex++;
                        });

                        /* Apply the tooltips */
                        $('#surveyDataTable thead th[title]').tooltip(
                        {
                            "container": 'body'
                        });
                    },
                    buttons: [
                        
                        $.extend(true, {}, buttonCommon, {
                            extend: 'excelHtml5',
							text: 'Extract',
                            className: 'btn green btn-outline',
                            exportOptions: {
                                columns: exportColumns
                            },
                            customize: function(xlsx) {
                                var sheet = dataexport.xl.worksheets['sheet1.xml'];
 
                                $('row c[r^="D"]', sheet).each(function () {
                                    if ($('is t', this).html() == "<img src='" + Config.getAppRootUrl() + "/images/flag_red.png' />") {
                                        $('is t', this).text(1);
                                    } else {
                                        $('is t', this).text('0');
                                    }
                                });
                                $('row c[r^="E"]', sheet).each(function () {
                                    if ($('is t', this).html() == "<img src='" + Config.getAppRootUrl() + "/images/flag_yellow.png' />") {
                                        $('is t', this).text(1);
                                    } else {
                                        $('is t', this).text('0');
                                    }
                                });
                            }
                        })
                    ],
                    drawCallback: function (settings) {
                        $("#surveyDataTable").wrap("<div class='table-responsive'></div>"); // this makes the table fit into the div
                    },
                    responsive: {
                        details: {

                        }
                    },
                    "order": [
                        [0, 'asc']
                    ],
                    // set the initial value
                    "pageLength": 25,
                    "dom": "<'row' <'col-md-12'B>><'row'<'col-md-6 col-sm-12'l><'col-md-6 col-sm-12'f>r><'table-scrollable't><'row'<'col-md-5 col-sm-12'i><'col-md-7 col-sm-12'p>>", // horizobtal scrollable datatable
                        
                });
                
            } else {
                $('#message-div').removeClass('display-hide').removeClass('alert-danger').addClass('alert-success');
                $('#message-div').html('<h4><i class="fa fa-bullhorn"></i> Attention </h4> There are no respondents to the survey in the chosen period.');
            }

        }
    };

}();


jQuery(document).ready(function () {
    
    //check if authenticated
    AccessControl.init();

    HaloViewData.initDateRange();  
    HaloViewData.init();

    $('#btn-search').click(function () {
        let surveyID = '';
        if (typeof (Storage) !== "undefined") {
            surveyID = localStorage.getItem("halo_current_surveyid");
        }else{
            window.location.href = Config.getLoginPageUrl();
        }
        let requestData = HaloViewData.buildRequestData(surveyID);
        HaloViewData.getViewData(surveyID, requestData);
    });

});



