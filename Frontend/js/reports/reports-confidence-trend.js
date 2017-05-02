var ReportsConfidenceTrend = function () {
    return {
        init : function (surveyID){
            if (surveyID) {                
                $.ajax({
                    beforeSend: function () {
                        Utils.blockUI({
                            target: '#confidence-trend-div',
                            message: 'Loading Confidence Trend Chart. <br/>Please wait...', 
                            boxed : true
                        });
                    },
                    complete: function () {
                        Utils.unblockUI('#confidence-trend-div');
                    },
                    url: Config.getApiRootUrl() + '/reports/confidencetrenddata/' + surveyID,
                    success: function (json) {
                        if (json && (json.HasEnoughDataPoints === true))
                        {
                            noTrendChart = false;
                            $('#trend-top-page-break').addClass("page-breaker");
                            $('#no-trend-data-div').removeClass("display-hide");
                            $('#confidence-trend-left').removeClass("display-hide");
                            $('#confidence-trend-center').removeClass("display-hide");
                            $('#confidence-trend-right').removeClass("display-hide");
                            $('#trend-bottom-page-break').addClass("page-breaker");
                            ReportsConfidenceTrend.initChartConfidenceTrend(json.DataPoints);
                        }
                        else {
                            noTrendChart = true;
                            $('#trend-top-page-break').removeClass("page-breaker");
                            $('#no-trend-data-div').html("<p>&nbsp;</p><p style='text-align:center'> INSUFFICIENT DATA IN THE LAST 6 MONTHS</p>");
                            $('#no-trend-data-div').removeClass("display-hide");
                            $('#confidence-trend-left').addClass("display-hide");
                            $('#confidence-trend-center').addClass("display-hide");
                            $('#confidence-trend-right').addClass("display-hide");
                            $('#trend-bottom-page-break').addClass("page-breaker");
                        }
                    },
                    failure: function (errMsg) {
                        alert(errMsg); //TODO; Use Modal
                    }
                });
            }
        },
        initChartConfidenceTrend: function (data) {
            let confidenceTrendChart = AmCharts.makeChart("confidence_trend_chart", {
                "type": "serial",
                "dataProvider": data,
                "categoryField": "Period",
                "categoryAxis": {
                    "gridAlpha": 0,
                },
                "valueAxes": [{
                    "axisAlpha": 1,
                    "position": "left",
                    "title": "percentage (%)",
                    "gridAlpha": 0,
                    minimum: 0,
                    maximum: 100
                }],
                "graphs": [{
                    "title": "percentage",
                    "valueField": "Current",
                    "type": "line",
                    "lineColorField": "lineColor",
                    "fillColorsField": "lineColor",
                    "fillColors": "#75297F",
                    "lineColor": "#75297F",
                    "fillAlphas": 0.3,
                    "balloonText": "[[value]]",
                    "lineThickness": 1,
                    "legendValueText": "[[value]]",
                    "bullet": "square",
                    "bulletBorderThickness": 1,
                    "bulletBorderAlpha": 1,
                }],
            });           
        }
    };

}();