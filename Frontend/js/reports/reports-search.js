var ReportsSearch = function () {
    return {
        
        setOptions: function (data, selectControl, selectGroup) {
            var htmlOptions = '';
            var total = 0;
            
            $.each(data, function (key, obj) {
                if (obj.length < 5) {
                    htmlOptions += '<option value=' + '"' + key + '" disabled>' + key + ' (' + obj.length + ')' + '</option>';
                } else {
                    htmlOptions += '<option value=' + '"' + key + '">' + key + ' (' + obj.length + ')' + '</option>';
                }
                total += obj.length;
            });
            if (total < 1) {
                htmlOptions = '';
            }
            else {
                htmlOptions =  '<option value= "All">All (' + total + ') </option>' + htmlOptions;
            }
            if (htmlOptions == '') {
                $('#' + selectControl).attr('disabled', 'disabled');
                $('#' + selectGroup).addClass('display-hide');
            } else {
                $('#'+ selectControl).append(htmlOptions);
                $('#' + selectControl).removeAttr('disabled');
                $('#' + selectGroup).removeClass('display-hide');
            }
            ReportsSearch.sortSelect('#' + selectControl);
        },
        sortSelect: function (select) {
            var my_options = $(select + " option");
            var selected = $(select).val(); /* preserving original selection, step 1 */

            my_options.sort(function (a, b) {
                if (a.text > b.text) return 1;
                else if (a.text < b.text) return -1;
                else return 0
            })

            $(select).empty().append(my_options);
            $(select).val(selected); /* preserving original selection, step 2 */
        },        
        clearAllWorkOrderControls: function () {
            $('#location1').find('option').remove().end();
            $('#location2').find('option').remove().end();
            $('#location3').find('option').remove().end();
            $('#location4').find('option').remove().end();
            $('#sla').find('option').remove().end();
            $('#slaBreach').find('option').remove().end();
            $('#slaContract').find('option').remove().end();
            $('#slaService').find('option').remove().end();
            $('#slaServiceType').find('option').remove().end();
            $('#failure').find('option').remove().end();
            $('#problem').find('option').remove().end();
            $('#cause').find('option').remove().end();
            $('#remedy').find('option').remove().end();
        },
        hideAllWorkOrderControls: function () {
            $('#location1-group').addClass('display-hide');
            $('#location2-group').addClass('display-hide');
            $('#location3-group').addClass('display-hide');
            $('#location4-group').addClass('display-hide');
            $('#sla-group').addClass('display-hide');
            $('#slaBreach-group').addClass('display-hide');
            $('#slaContract-group').addClass('display-hide');
            $('#slaService-group').addClass('display-hide');
            $('#slaServiceType-group').addClass('display-hide');
            $('#failure-group').addClass('display-hide');
            $('#problem-group').addClass('display-hide');
            $('#cause-group').addClass('display-hide');
            $('#remedy-group').addClass('display-hide');
        },        
        getWorkOrderFields: function (workOrderArray) {
            let workOrderFields = [];
            $.each(workOrderArray, function (i, workOrder) {
                if((workOrder.LocationLevel1 != null) && (workOrder.LocationLevel1 != ''))
                {
                    workOrderFields.push({ ControlId: 'location1', Div: 'location1-group' });
                    return false;
                }
            });
            $.each(workOrderArray, function (i, workOrder) {
                if ((workOrder.LocationLevel2 != null) && (workOrder.LocationLevel2 != '')) {
                    workOrderFields.push({ ControlId: 'location2', Div: 'location2-group' });
                    return false;
                }
            });
            $.each(workOrderArray, function (i, workOrder) {
                if ((workOrder.LocationLevel3 != null) && (workOrder.LocationLevel3 != '')) {
                    workOrderFields.push({ ControlId: 'location3', Div: 'location3-group' });
                    return false;
                }
            });
            $.each(workOrderArray, function (i, workOrder) {
                if ((workOrder.LocationLevel4 != null) && (workOrder.LocationLevel4 != '')) {
                    workOrderFields.push({ ControlId: 'location4', Div: 'location4-group' });
                    return false;
                }
            });
            $.each(workOrderArray, function (i, workOrder) {
                if ((workOrder.SLA != null) && (workOrder.SLA != '')) {
                    workOrderFields.push({ ControlId: 'sla', Div: 'sla-group' });
                    return false;
                }
            });
            $.each(workOrderArray, function (i, workOrder) {
                if ((workOrder.SLABreach != null) && (workOrder.SLABreach != '')) {
                    workOrderFields.push({ ControlId: 'slaBreach', Div: 'slaBreach-group' });
                    return false;
                }
            });
            $.each(workOrderArray, function (i, workOrder) {
                if ((workOrder.SLAContract != null) && (workOrder.SLAContract != '')) {
                    workOrderFields.push({ ControlId: 'slaContract', Div: 'slaContract-group' });
                    return false;
                }
            });
            $.each(workOrderArray, function (i, workOrder) {
                if ((workOrder.SLAService != null) && (workOrder.SLAService != '')) {
                    workOrderFields.push({ ControlId: 'slaService', Div: 'slaService-group'});
                    return false;
                }
            });
            $.each(workOrderArray, function (i, workOrder) {
                if ((workOrder.SLAServiceType != null) && (workOrder.SLAServiceType != '')) {
                    workOrderFields.push({ ControlId: 'slaServiceType', Div: 'slaServiceType-group' });
                    return false;
                }
            });
            $.each(workOrderArray, function (i, workOrder) {
                if ((workOrder.Cause != null) && (workOrder.Cause != '')) {
                    workOrderFields.push({ ControlId: 'cause', Div: 'cause-group' });
                    return false;
                }
            });
            $.each(workOrderArray, function (i, workOrder) {
                if ((workOrder.Failure != null) && (workOrder.Failure != '')) {
                    workOrderFields.push({ ControlId: 'failure', Div: 'failure-group' });
                    return false;
                }
            });
            $.each(workOrderArray, function (i, workOrder) {
                if ((workOrder.Problem != null) && (workOrder.Problem != '')) {
                    workOrderFields.push({ ControlId: 'problem', Div: 'problem-group' });
                    return false;
                }
            });
            $.each(workOrderArray, function (i, workOrder) {
                if ((workOrder.Remedy != null) && (workOrder.Remedy != '')) {
                    workOrderFields.push({ ControlId: 'remedy', Div: 'remedy-group' });
                    return false;
                }
            });
            return workOrderFields;
        },
        loadControlOptions: function (workOrderControl) {
            let retrievedFilters = localStorage.getItem('halo_survey_workorders');
            let workOrderFilters = JSON.parse(retrievedFilters);
            let workOrderGroups = [];
            if(workOrderControl.ControlId == 'location1')
            {
                workOrderGroups = _.groupBy(workOrderFilters, function (d) { return d.LocationLevel1 });
            }
            if (workOrderControl.ControlId == 'location2') {
                workOrderGroups = _.groupBy(workOrderFilters, function (d) { return d.LocationLevel2 });
            }
            if (workOrderControl.ControlId == 'location3') {
                workOrderGroups = _.groupBy(workOrderFilters, function (d) { return d.LocationLevel3 });
            }
            if (workOrderControl.ControlId == 'location4') {
                workOrderGroups = _.groupBy(workOrderFilters, function (d) { return d.LocationLevel4 });
            }
            if (workOrderControl.ControlId == 'sla') {
                workOrderGroups = _.groupBy(workOrderFilters, function (d) { return d.SLA });
            }
            if (workOrderControl.ControlId == 'slaBreach') {
                workOrderGroups = _.groupBy(workOrderFilters, function (d) { return d.SLABreach });
            }
            if (workOrderControl.ControlId == 'slaContract') {
                workOrderGroups = _.groupBy(workOrderFilters, function (d) { return d.SLAContract });
            }
            if (workOrderControl.ControlId == 'slaService') {
                workOrderGroups = _.groupBy(workOrderFilters, function (d) { return d.SLAService });
            }
            if (workOrderControl.ControlId == 'slaServiceType') {
                workOrderGroups = _.groupBy(workOrderFilters, function (d) { return d.SLAServiceType });
            }
            if (workOrderControl.ControlId == 'cause') {
                workOrderGroups = _.groupBy(workOrderFilters, function (d) { return d.Cause });
            }
            if (workOrderControl.ControlId == 'failure') {
                workOrderGroups = _.groupBy(workOrderFilters, function (d) { return d.Failure });
            }
            if (workOrderControl.ControlId == 'problem') {
                workOrderGroups = _.groupBy(workOrderFilters, function (d) { return d.Problem });
            }
            if (workOrderControl.ControlId == 'remedy') {
                workOrderGroups = _.groupBy(workOrderFilters, function (d) { return d.Remedy });
            }
			

            // Clear before loading
            $('#' + workOrderControl.Div).find('option').remove().end();
            ReportsSearch.setOptions(workOrderGroups, workOrderControl.ControlId, workOrderControl.Div);
        },        
        loadControlOptionsWithSelection: function (workOrderControl) {
            //TODO: THIS MUST FILTER WITH SELECTION OPTIONS OF PRIOR SELECT CONTROLS BEFRO GROUPING
            let retrievedFilters = localStorage.getItem('halo_survey_workorders');
            let workOrderFilters = JSON.parse(retrievedFilters);
            let workOrderGroups = [];

            let selectedLocation1 = $("#location1 option:selected").val();
            let selectedLocation2 = $("#location2 option:selected").val();
            let selectedLocation3 = $("#location3 option:selected").val();
            let selectedLocation4 = $("#location4 option:selected").val();
            let selectedSLA = $("#sla option:selected").val();
            let selectedSLABreach = $("#slaBreach option:selected").val();
            let selectedSLAContract = $("#slaContract option:selected").val();
            let selectedService = $("#slaService option:selected").val();
            let selectedServiceType = $("#slaServiceType option:selected").val();
            let failure = $("#failure option:selected").val();
            let problem = $("#problem option:selected").val();
            let cause = $("#cause option:selected").val();
            let remedy = $("#remedy option:selected").val();


            let filteredOnLocation1 = _.filter(workOrderFilters, function (workorder) {
                return workorder.LocationLevel1 == selectedLocation1;
            });
            let filteredOnLocation2 = _.filter(filteredOnLocation1, function (workorder) {
                if (selectedLocation2 == 'All') {
                    return filteredOnLocation1;
                } else {
                    return workorder.LocationLevel2 == selectedLocation2;
                }
            });
            let filteredOnLocation3 = _.filter(filteredOnLocation2, function (workorder) {
                if (selectedLocation3 == 'All') {
                    return filteredOnLocation2;
                } else {
                    return workorder.LocationLevel3 == selectedLocation3;
                }
            });
            let filteredOnLocation4 = _.filter(filteredOnLocation3, function (workorder) {
                if (selectedLocation4 == 'All') {
                    return filteredOnLocation3;
                } else {
                    return workorder.LocationLevel4 == selectedLocation4;
                }
            });
            let filteredOnSLA = _.filter(filteredOnLocation4, function (workorder) {
                if (selectedSLA == 'All') {
                    return filteredOnLocation4;
                } else {
                    return workorder.SLA == selectedSLA;
                }
            });            
            let filteredOnSLABreach = _.filter(filteredOnSLA, function (workorder) {
                if (selectedSLABreach == 'All') {
                    return filteredOnSLA;
                } else {
                    return workorder.SLABreach == selectedSLABreach;
                }
            });
            let filteredOnSLAContract = _.filter(filteredOnSLABreach, function (workorder) {
                if (selectedSLAContract == 'All') {
                    return filteredOnSLABreach;
                } else {
                    return workorder.SLABContract == selectedSLAContract;
                }
            });
            let filteredOnSLAService = _.filter(filteredOnSLAContract, function (workorder) {
                if (selectedSLAService == 'All') {
                    return filteredOnSLAContract;
                } else {
                    return workorder.SLAService == selectedSLAService;
                }
            });
            let filteredOnSLAServiceType = _.filter(filteredOnSLAService, function (workorder) {
                if (selectedSLAServiceType == 'All') {
                    return filteredOnSLAService;
                } else {
                    return workorder.SLAServiceType == selectedSLAServiceType;
                }
            });
            let filteredOnFailure = _.filter(filteredOnSLAServiceType, function (workorder) {
                if (selectedFailure == 'All') {
                    return filteredOnSLAServiceType;
                } else {
                    return workorder.Failure == selectedFailure;
                }
            });
            let filteredOnProblem = _.filter(filteredOnFailure, function (workorder) {
                if (selectedProblem == 'All') {
                    return filteredOnFailure;
                } else {
                    return workorder.Problem == selectedProblem;
                }
            });
            let filteredOnCause = _.filter(filteredOnProblem, function (workorder) {
                if (selectedCause == 'All') {
                    return filteredOnProblem;
                } else {
                    return workorder.Cause == selectedCause;
                }
            });
            let filteredOnRemedy = _.filter(filteredOnProblem, function (workorder) {
                if (selectedRemedy == 'All') {
                    return filteredOnProblem;
                } else {
                    return workorder.Remedy == selectedRemedy;
                }
            });

            if(workOrderControl.ControlId == 'location1')
            {                
                workOrderGroups = _.groupBy(workOrderFilters, function (d) { return d.LocationLevel1 });
            }
            if (workOrderControl.ControlId == 'location2') {
                workOrderGroups = _.groupBy(filteredOnLocation1, function (d) { return d.LocationLevel2 });
            }
            if (workOrderControl.ControlId == 'location3') {
                workOrderGroups = _.groupBy(filteredOnLocation2, function (d) { return d.LocationLevel3 });
            }
            if (workOrderControl.ControlId == 'location4') {
                workOrderGroups = _.groupBy(filteredOnLocation3, function (d) { return d.LocationLevel4 });
            }
            if (workOrderControl.ControlId == 'sla') {
                workOrderGroups = _.groupBy(filteredOnLocation4, function (d) { return d.SLA });
            }
            if (workOrderControl.ControlId == 'slaBreach') {
                workOrderGroups = _.groupBy(filteredOnSLA, function (d) { return d.SLABreach });
            }
            if (workOrderControl.ControlId == 'slaContract') {
                workOrderGroups = _.groupBy(filteredOnSLABreach, function (d) { return d.SLAContract });
            }
            if (workOrderControl.ControlId == 'slaService') {
                workOrderGroups = _.groupBy(filteredOnSLAContract, function (d) { return d.SLAService });
            }
            if (workOrderControl.ControlId == 'slaServiceType') {
                workOrderGroups = _.groupBy(filteredOnSLAService, function (d) { return d.SLAServiceType });
            }
            if (workOrderControl.ControlId == 'cause') {
                workOrderGroups = _.groupBy(filteredOnSLAServiceType, function (d) { return d.Cause });
            }
            if (workOrderControl.ControlId == 'failure') {
                workOrderGroups = _.groupBy(filteredOnCause, function (d) { return d.Failure });
            }
            if (workOrderControl.ControlId == 'problem') {
                workOrderGroups = _.groupBy(filteredOnFailure, function (d) { return d.Problem });
            }
            if (workOrderControl.ControlId == 'remedy') {
                workOrderGroups = _.groupBy(filteredOnProblem, function (d) { return d.Remedy });
            }
			
            // Clear before loading
            $('#' + workOrderControl.Div).find('option').remove().end();
            ReportsSearch.setOptions(workOrderGroups, workOrderControl.ControlId, workOrderControl.Div);
        },
        disableLowerSelectControls: function (selectControlId) {
            var workOrderFiltersControls = ["location1", "location2", "location3", "location4",
                                            "sla", "slaBreach", "slaContract", "slaService", "slaServiceType",
                                            "failure", "problem", "cause", "remedy"];
            var disable = false;
            $.each(workOrderFiltersControls, function (index, item) {
                if (disable) {
                    $('#' + item).find('option').remove().end();
                    $('#' + item).attr('disabled', 'disabled');
                }
                if(selectControlId == item)
                {
                    disable = true;
                }
            });
        },
        getHigherSelectControls: function (selectControlId) {
            let workOrderFiltersControls = ["location1", "location2", "location3", "location4",
                                           "sla", "slaBreach", "slaContract", "slaService", "slaServiceType",
                                           "failure", "problem", "cause", "remedy"];
            let controlsAbove = [];
            if (selectControlId == "location1")
            {
                return controlsAbove;
            }
            let include = true;
            $.each(workOrderFiltersControls, function (index, controlId) {
                if (include == true) {
                    let selectedOption = $('#' + controlId + " option:selected").val();
                    controlsAbove.push({ ControlId: controlId, Div: controlId + "-group", SelectedOption: selectedOption });
                }
                if (selectControlId == controlId) {
                    include = false;
                    return controlsAbove;
                }
            });
            return controlsAbove;
        },
        getImmediateLowerSelectControl: function (selectControlId) {
            let ctoReturn = {};
            let retrievedWorkOrders = localStorage.getItem('halo_survey_workorders');
            let workOrders = JSON.parse(retrievedWorkOrders);
            let workOrderFiltersControls = ReportsSearch.getWorkOrderFields(workOrders);
            
            let pick = false;
            $.each(workOrderFiltersControls, function (index, control) {
                if (pick == true) {
                    ctoReturn = control;
                    return false;
                }
                if (selectControlId == control.ControlId) {
                    pick = true;
                }
            });
            return ctoReturn;
        },
        showWorkOrderControls: function (controlsToShow) {
            $.each(controlsToShow, function (i, control) {
                $('#'+control.Div).removeClass('display-hide');
            });
        },
        clearFilterQuestionControls: function () {
            $('#filterQuestion1').find('option').remove().end();
            $('#filterQuestion2').find('option').remove().end();
        },
        hideAllFilterQuestionControls: function () {
            $('#filterQuestion1-group').addClass('display-hide');
            $('#filterQuestion2-group').addClass('display-hide');
        },
        updateMabeyHireFilters : function (surveyID){            
            if (surveyID == 499)
            {
                $('#location1-label').text("Depot Location");
                $('#location2-label').text("Sales Rep") ;
            }
        },
        setUpWorkOrderFilters : function (surveyStats) {
            $('#all-report-span').text(' (' + surveyStats.TotalNumberOfRespondents + ')');
            $('#without-filters-report-span').text(' (' + (surveyStats.TotalNumberOfRespondents - surveyStats.WorkOrderRespondents) + ')');
            $('#with-filters-report-span').text(' (' + surveyStats.WorkOrderRespondents + ')');
            if ((surveyStats.TotalNumberOfRespondents - surveyStats.WorkOrderRespondents) <  5) {
                $('#withoutFiltersReport').attr('disabled', 'disabled');
            }
            if (surveyStats.WorkOrderRespondents < 5) {
                $('#withFiltersReport').attr('disabled', 'disabled');
            }
            //Load work order select controls
            // Clear content of all workorder controls
            ReportsSearch.clearAllWorkOrderControls();
            // Hide all workorder controls
            ReportsSearch.hideAllWorkOrderControls();
            // Get work order fields with records
            let workOrderControlsToShow = ReportsSearch.getWorkOrderFields(surveyStats.WorkOrders);
            
            if (workOrderControlsToShow.length > 0)
            {
                //Load first control in the list
                let firstControl = workOrderControlsToShow[0];
                
                ReportsSearch.loadControlOptions(firstControl);
                // Disable the rest
                ReportsSearch.disableLowerSelectControls(workOrderControlsToShow[0].ControlId);

                // Show filters div
                $('report-filters-div').removeClass('display-hide');
                $('report-filters-div').removeAttr('style');
                // Show/Hide controls as required.
                ReportsSearch.showWorkOrderControls(workOrderControlsToShow);                
            }
        },
        setUpCustomFilters : function (surveyStats) {        
            $('#contract-filters-div').addClass('display-hide');
            //Show the filter questions
            $('#filter-question-div').removeClass('display-hide');
            // clear filter question controls
            ReportsSearch.clearFilterQuestionControls();
            // Hide all filter question controls
            ReportsSearch.hideAllFilterQuestionControls();

            //Load and show first
            $('#filterQuestion1-group').removeClass('display-hide');
            $('#filterQuestion1-label').text(surveyStats.FilterQuestions[0].Question);
            $('#filterQuestion1-promptid').text(surveyStats.FilterQuestions[0].PromptID);
            let htmlOptions = '';
            let total = 0;
            $.each(surveyStats.FilterQuestions[0].Options, function (i, option) {
                if (option.Count < 5) {
                    htmlOptions += '<option value= "' + option.Value + '" disabled>' + option.Option + ' (' + option.Count + ')' + '</option>';
                } else {
                    htmlOptions += '<option value= "' + option.Value + '">' + option.Option + ' (' + option.Count + ')' + '</option>';
                }
                total += option.Count;
            });
            htmlOptions = '<option value= "All">All (' + total + ') </option>' + htmlOptions;
            $('#filterQuestion1').append(htmlOptions);
            $('#filterQuestion1').removeAttr('disabled');

            //Load and show second disabled
            if ((surveyStats.FilterQuestions.length > 1) && (surveyStats.FilterQuestions[1] != null)) {
                $('#filterQuestion2-group').removeClass('display-hide');
                $('#filterQuestion2-label').text(surveyStats.FilterQuestions[1].Question);
                $('#filterQuestion2-promptid').text(surveyStats.FilterQuestions[1].PromptID);
                let htmlOptions = '';
                let total = 0;
                $.each(surveyStats.FilterQuestions[1].Options, function (i, option) {
                    if (option.Count < 5) {
                        htmlOptions += '<option value= "' + option.Value + '" disabled>' + option.Option + ' (' + option.Count + ')' + '</option>';
                    } else {
                        htmlOptions += '<option value= "' + option.Value + '">' + option.Option + ' (' + option.Count + ')' + '</option>';
                    }
                    total += option.Count;
                });
                htmlOptions = '<option value= "All">All (' + total + ') </option>' + htmlOptions;
                $('#filterQuestion2').append(htmlOptions);
                $('#filterQuestion2').removeAttr('disabled');
            }

            $('#filterQuestion1').on('change', function () {
                var retrievedItems = localStorage.getItem("halo_survey_filterresponses");
                var unfilteredResponses = JSON.parse(retrievedItems);
                
                var filteredObjects = _.filter(unfilteredResponses, function (response) {
                    return response.ValueResponse1 == $('#filterQuestion1').val();
                });
                
                var optionsTwo = [];
                var groupedObjects = [];
                if ($('#filterQuestion1').val() == "All") {
                    $.each(unfilteredResponses, function (i, obj) {
                        optionsTwo.push({ "Option": obj.Option2, "Value": obj.ValueResponse2 });
                    });
                } else {
                    //var valuesTwo = [];
                    $.each(filteredObjects, function (i, obj) {
                        optionsTwo.push({ "Option": obj.Option2, "Value": obj.ValueResponse2 });
                    });
                }

                groupedObjects = _.groupBy(optionsTwo, function (d) { return d.Value });
                
                
                //Reload and show second
                // first clear
                $('#filterQuestion2').find('option').remove().end();
                var htmlOptions2 = '';
                var total = 0;
                $.each(groupedObjects, function (i, option) {
                    if (option.length < 5) {
                        htmlOptions2 += '<option value= "' + option[0].Value + '" disabled>' + option[0].Option + ' (' + option.length + ')' + '</option>';
                    } else {
                        htmlOptions2 += '<option value= "' + option[0].Value + '">' + option[0].Option + ' (' + option.length + ')' + '</option>';
                    }
                    total += option.length;
                });
                htmlOptions2 = '<option value= "All">All (' + total + ') </option>' + htmlOptions2;
                $('#filterQuestion2').append(htmlOptions2);
                $('#filterQuestion2').removeAttr('disabled');
            });
        }
    };
}();


jQuery(document).ready(function () {
    
    //check if authenticated
    AccessControl.init();

    // Set default dates
    $('#from').val(moment().subtract(1, 'day').subtract(1, 'month').format("DD/MM/YYYY"));
    $('#to').val(moment().subtract(1, 'day').format("DD/MM/YYYY"));
    

    $('#btn-search').click(function () {
        // Hide all things first
        $('#contract-filters-div').addClass('display-hide');
        $('#report-body').attr('style', 'display:none');
        $('#filter-question-div').addClass('display-hide');
        $('#numberOfResponses-div').html("");

        //Validate date range
        if(($('#from').val() == '') || ($('#to').val() == ''))
        {
            $('#message-div').removeClass('display-hide').addClass('alert-danger');
            $('#message-div').html('<h4><i class="fa fa-warning"></i> Error </h4> Please enter date range');
        } else {
            $('#message-div').addClass('display-hide');
            //fetch 
            let surveyID = '';
            if (typeof (Storage) !== "undefined") {
                surveyID = localStorage.getItem("halo_current_surveyid");
                //HACK: IF MABEY HIRE CHANGE THE LABELS
                ReportsSearch.updateMabeyHireFilters(surveyID);

                if (isNaN(surveyID) || surveyID < 1) {
                    window.location.href = 'client-login.html'; //send back to login page
                }
                else {
                    let dateRange = ReportsBuilder.getDateRange();
                    
                    $.ajax({
                        url: Config.getApiRootUrl() + '/reports/surveystats/' + surveyID + dateRange,
                        beforeSend: function () {
                            Utils.blockUI({
                                target: '#date-range-div',
                                message: 'Please wait...', 
                                boxed : true
                            });
                        },
                        complete: function () {
                            Utils.unblockUI('#date-range-div');
                        },
                        success: function (survey) {
                            if(survey && survey.Statistics) 
                            {
                                let surveyStats =  survey.Statistics;                                
                                localStorage.removeItem('halo_survey_workorders');
                                localStorage.removeItem('halo_survey_filterquestions');
                                localStorage.removeItem('halo_survey_filterresponses');
                                localStorage.setItem('halo_survey_workorders', JSON.stringify(surveyStats.WorkOrders));
                                localStorage.setItem('halo_survey_filterquestions', JSON.stringify(surveyStats.FilterQuestions));
                                localStorage.setItem('halo_survey_filterresponses', JSON.stringify(surveyStats.FilterResponses));
                                if (surveyStats.TotalNumberOfRespondents < 5) {
                                    $('#filter-question-div').addClass('display-hide');
                                    $('#contract-filters-div').addClass('display-hide');
                                    $('#build-report-div').addClass('display-hide');
                                    $('#message-div').removeClass('display-hide').removeClass('alert-danger').addClass('alert-success');
                                    $('#message-div').html('<h4><i class="fa fa-bullhorn"></i> Attention </h4> Not enough respondents for the survey in the chosen period.');
                                } else {
                                    $('#report-header').html(surveyStats.SurveyName + ' Report');
                                    $('#logo-div').html('<img id="survey-logo" src="' + Config.getHaloImgPath() + '\\Logos\\' + surveyStats.LogoFile + '"' + ' alt="logo" />');
                                    $('#message-div').addClass('display-hide');
                                    $('#build-report-div').removeClass('display-hide');
                                    $('#contract-filters-div').removeClass('display-hide');
                                    if (surveyStats.HasWorkOrders) {
                                        ReportsSearch.setUpWorkOrderFilters(surveyStats);
                                    } // End if HasWorkOrder 
                                    else if (surveyStats.HasFilterQuestions) {
                                        ReportsSearch.setUpCustomFilters(surveyStats);
                                    }// End If HasFilterQuestions
                                    else {
                                        $('#contract-filters-div').addClass('display-hide');
                                        $('#numberOfResponses-div').html("Total  number of responses: " + surveyStats.TotalNumberOfRespondents );
                                    }
                                } // End If  at least 5
                            } // End if(survey && survey.Statistics) 
                            else
                            {
                                console.log("COULD NOT GET Survey Statistics FROM API")
                            }
                        }
                    });
                }
            }
        }

    });
    
    $('input[name=radiobtn-responses]').click(function () {
        if ($('input[name=radiobtn-responses]:checked').val() == 'with')
        {
            $('#report-filters-div').removeClass('display-hide');
        } else {
            $('#report-filters-div').addClass('display-hide');
        }
    });

    $('#filter-form-div').on("click", '#submitForm', function (e) {
        $("#filter-form").submit();
    });
    
    $('.workOrderSelect').change(function () {        
        let selectedOption = $('#' + this.id + " option:selected").val();
        // Disable all controls below one selected
        ReportsSearch.disableLowerSelectControls(this.id);

        // Get Selected option in all controls above this on
        let priorControls = ReportsSearch.getHigherSelectControls(this.id);

        //START FROM HERE getImmediateLowerSelectControl NOT WORKING
        let nextControl = ReportsSearch.getImmediateLowerSelectControl(this.id);
        
        //TODO: Need to be able to use selected option in PRIOR AND THIS control to load next control
        ReportsSearch.loadControlOptionsWithSelection(nextControl);

        // Disable all controls below [NOT SURE THIS NEXT LINE IS NEEDED AS IT WAS ALREADY CALLED ABOVE]
        ReportsSearch.disableLowerSelectControls(nextControl.ControlId);
    });


});