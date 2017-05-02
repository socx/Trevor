var ReportsCustomerExperience = function () {    
    
    let drawCustomerExperienceChart = function (div, data) {
        AmCharts.makeChart(div, {
            "type": "serial",
            "theme": "light",
            "dataProvider": data.DynamicPercentages,
            "columnSpacing": 6,
            "graphs": [{
                "lineColor": "#1a81ae",
                "fillAlphas": 1,
                "type": "column",
                "valueField": "Current",
                "title": "Current",
                "balloonText": "<span style='font-size:16px; font-weight:bold'>[[value]]%</span> : [[additional]]</span><br><span style='font-size:12px;'>gave a score of [[category]]"
            }, {
                "lineColor": "#f7921e",
                "fillAlphas": 1,
                "type": "column",
                "valueField": "Previous",
                "title": "Previous",
                "balloonText": "<span style='font-size:16px; font-weight:bold'>[[value]]%</span> : [[additional]]</span><br><span style='font-size:12px;'>gave a score of [[category]]"
            }, {
                "lineColor": "#1a81ae",
                "lineAlpha": 0,
                "bullet": "square",
                "bulletSize": 24,
                "valueField": "CurrentMeanValue",
                "labelText": "[[CurrentMean]]",
                "labelOffset": -17,
                "color": "#fff",
                "visibleInLegend": false,
                "balloonText": "mean score : <br><span style='font-size:12px; font-weight:bold;'> [[CurrentMean]]"
            }, {
                "lineColor": "#f7921e",
                "lineAlpha": 0,
                "bullet": "square",
                "bulletSize": 24,
                "valueField": "PreviousMeanValue",
                "labelText": "[[PreviousMean]]",
                "labelOffset": -17,
                "color": "#fff",
                "visibleInLegend": false,
                "balloonText": "mean score : <br><span style='font-size:12px; font-weight:bold;'> [[PreviousMean]]"
            }, {
                "lineColor": "#f7921e",
                "fillAlphas": 1,
                "valueField": "PreviousMeanValue",
                "type": "column",
                "clustered": false,
                "fixedColumnWidth": 3,
                "visibleInLegend": false,
                "balloonText": "mean score : <br><span style='font-size:12px; font-weight:bold;'> [[PreviousMean]]"
            }, {
                "lineColor": "#1a81ae",
                "fillAlphas": 1,
                "valueField": "CurrentMeanValue",
                "type": "column",
                "clustered": false,
                "fixedColumnWidth": 3,
                "visibleInLegend": false,
                "balloonText": "mean score : <br><span style='font-size:12px; font-weight:bold;'> [[CurrentMean]]"
            }],
            "categoryField": "Score",
            "valueAxes": [{
                "axisAlpha": 1,
                "position": "left",
                "title": "percentage (%)",
                "gridAlpha": 0,
                precision: 0
            }],
            "categoryAxis": {
                "gridAlpha": 0,
            },
            "responsive": {
                "enabled": true
            }
        });
    }

    return {
        init : function (queryString){
            if (queryString) {
                $.ajax({
                    beforeSend: function () {
                        Utils.blockUI({
                            target: '#customer-experience-div',
                            message: 'Loading Customer Experience Chart(s). <br/>Please wait...', 
                            boxed : true
                        });
                    },
                    complete: function () {
                        Utils.unblockUI('#customer-experience-div');
                    },
                    url: Config.getApiRootUrl() + '/reports/customerexperiencedata' + '?' + queryString,
                    success: function (json) {
                        ReportsCustomerExperience.initChartCustomerExperience(json);
                    }
                });
            }
        },
        initChartCustomerExperience: function (data) {
            $('#customer-experience-chart-div').empty();
            let periodHtml = '<div class="col-md-12 col-sm-12">' +
                                '<p><span class="current-box">&nbsp;</span> Current Period (' + data.Periods.Current + ')<br /><span class="previous-box">&nbsp;</span>  Previous Period (' + data.Periods.Previous + ')</p>' +
                            '</div>';
            $('#customer-experience-chart-div').append($(periodHtml));
            let counter = 0;
            $.each(data.ChartData, function (key, value) {
                counter++;
                let id = 'customer_experience_chart_div' + (key + 1);
                let html = '<div class="col-md-12 col-sm-12"><div class="col-md-2 col-sm-2 col-xs-2 small">' +
                                "<br /><br /><br />" +
                                "<small><strong>" + value.SliderPrompts.Min + "</strong></small>" +
                            '</div>' +
                            '<div class="col-md-8 col-sm-8 col-xs-8">' +
                                '<div id=' + id + ' class="chart resizeable" style="width: 500px; height: 370px;"> </div>' +
                            '</div>' +
                            '<div class="col-md-2 col-sm-2 col-xs-2 small">' +
                                "<br /><br /><br />" +
                               " <small><strong>" + value.SliderPrompts.Max + "</strong></small>" +
                            '</div>' +
                        '</div>';
                  
                let tableId = 'customerexperiencetable' + (key + 1);
                let commentAnchorId = 'customerexperiencecommentanchor' + (key + 1);
                let tableHtml = '<div class="col-md-12 col-sm-12 col-xs-12 commenttable" >' +
                    "<a class='commentlink' id='" + commentAnchorId + "' onclick=\"" + "toggle_visibility('" + commentAnchorId + "', '" + tableId + "');" + '"' + ">Show comments for this graph</a>" +
                    '<table id="' + tableId + '" class="table table-condensed table-hover table-bordered table-toggle"></table>' +
                    '</div>';
                $('#customer-experience-chart-div').append($(html));
                $('#customer-experience-chart-div').append($(tableHtml));

                $('#' + tableId).DataTable({
                    data: value.Comments,
                    bDestroy: true,
                    bFilter: false, //hide Search bar
                    bPaginate: false,//hide pagination
                    bInfo: false, // hide showing entries
                    bSortable: false, //disable sorting
                    bLengthChange: false,
                    order: [[1, "asc"]],
                    autoWidth: false,
                    columns: [
                        { title: "Comment", data: "Comment", width: "80%", bSortable: false },
                        { title: "Score", data: "Score", width: "10%", bSortable: false, className: "dt-center" },
                        { title: " ", data: "RedFlag", width: "5%", bSortable: false },
                        { title: "", data: "AmberFlag", width: "5%", bSortable: false }
                    ]
                });
                drawCustomerExperienceChart(id, value);
                
                if(noTrendChart == true)
                {
                    if ((counter == 2) || (((counter - 2) % 3) == 0))
                    {
                        customerExperiencePageBroke = true;
                        $('#customer-experience-chart-div').append('<div style="page-break-after:always">&nbsp;</div>');
                    }else
                    {                        
                        customerExperiencePageBroke = false;
                    }
                }
                else
                {
                    if ((counter == 2) || (((counter - 2) % 3) == 0) || (data.ChartData.length == counter))
                    {
                        customerExperiencePageBroke = true;
                        $('#customer-experience-chart-div').append('<div style="page-break-after:always">&nbsp;</div>');
                    }else
                    {                        
                        customerExperiencePageBroke = false;
                    }
                }
            })
        },
    };

}();