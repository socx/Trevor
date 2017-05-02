var ReportsConfidenceScore = function () {
    
    let drawConfidenceScoreChart = function (div, value, bands) {
        let confidenceChart = AmCharts.makeChart(div, {
            "type": "gauge",
            "theme": "light",
            "axes": [{
                "axisThickness": 8,
                "axisAlpha": 0.2,
                "tickAlpha": 0.2,
                "valueInterval": 10,
                "bands": bands,
                "bottomText": "percent (%)",
                "bottomTextYOffset": -20,
                "endValue": 100
            }],
            "arrows": [{
                "color": "#75297F",
                "innerRadius": "2%",
                "nailRadius": 0,
                "radius": "90%"
            }],
            "export": {
                "enabled": true
            }
        });
        setTimeout(randomValue, 500);

        // set random value
        function randomValue() {
            if (confidenceChart 
                && confidenceChart.arrows 
                && confidenceChart.arrows[0]
                && confidenceChart.arrows[0].setValue) {
                confidenceChart.arrows[0].setValue(value);
                confidenceChart.axes[0].setBottomText(value + "%");
            }
        }
    };

    return {
        init : function (queryString){
            if (queryString) {
                $.ajax({
                    beforeSend: function () {
                        Utils.blockUI({
                            target: '#confidence-score-div',
                            message: 'Loading Confidence Score Data. <br/>Please wait...',
                            boxed : true
                        });
                    },
                    complete: function () {
                        Utils.unblockUI('#confidence-score-div');
                    },
                    url: Config.getApiRootUrl() + '/reports/confidencescoredata?' + queryString,
                    success: function (json) {
                        if(json)
                            ReportsConfidenceScore.initChartConfidenceScore(json);                    
                    },
                    failure: function (errMsg) {
                        alert(errMsg); //TODO; Use Modal
                    }
                });
            }
        },
        initChartConfidenceScore : function (confidenceScoreData) {
            let ragBands =[{
                "color": "#e82738",
                "endValue": 50,
                "startValue": 0
            }, {
                "color": "#f7921e",
                "endValue": 70,
                "startValue": 50
            }, {
                "color": "#1ea153",
                "endValue": 100,
                "innerRadius": "95%",
                "startValue": 70
            }];
            let purpleBand = [{
                "color": "#75297F",
                "endValue": 100,
                "innerRadius": "95%",
                "startValue": 0
            }];
            let fConfident = Math.round(confidenceScoreData.Current.FairlyConfident);
            let vConfident = Math.round(confidenceScoreData.Current.VeryConfident);
            let vAndFConfident = Math.round(confidenceScoreData.Current.FairlyConfident + confidenceScoreData.Current.VeryConfident);
            let nvConfident = Math.round(confidenceScoreData.Current.NotVeryConfident);
            let nalConfident = Math.round(confidenceScoreData.Current.NotAtAllConfident);
            let nConfident = Math.round(confidenceScoreData.Current.NotVeryConfident + confidenceScoreData.Current.NotAtAllConfident);
            drawConfidenceScoreChart("confidence_score_chart_1", vAndFConfident, ragBands);
            drawConfidenceScoreChart("confidence_score_chart_2", vConfident, purpleBand);

            $('#very-confident-respondents').text(vConfident + '%');
            $('#veryandfairly-confident-respondents').text(vAndFConfident + '%');
            $('#not-confident-respondents').text(nConfident + '%');

            let fConfident2 = Math.round(confidenceScoreData.AllTime.FairlyConfident);
            let vConfident2 = Math.round(confidenceScoreData.AllTime.VeryConfident);
            let vAndFConfident2 = Math.round(confidenceScoreData.AllTime.FairlyConfident + confidenceScoreData.AllTime.VeryConfident);
            let nvConfident2 = Math.round(confidenceScoreData.AllTime.NotVeryConfident);
            let nalConfident2 = Math.round(confidenceScoreData.AllTime.NotAtAllConfident);
            let nConfident2 = Math.round(confidenceScoreData.AllTime.NotVeryConfident + confidenceScoreData.AllTime.NotAtAllConfident);

            $('#alltime-very-confident-respondents').text(vConfident2 + '%');
            $('#alltime-veryandfairly-confident-respondents').text(vAndFConfident2 + '%');
            $('#alltime-not-confident-respondents').text(nConfident2 + '%');
        }
    };

}();