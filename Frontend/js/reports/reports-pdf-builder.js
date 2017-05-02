var reportDisplay = 2 // {1: iframe; 2: new tab}
var ReportsPdfBuilder = function () {
    return {
        init : function (request){
            if (request) {
                $.ajax({
                    beforeSend: function () {
                        Utils.blockUI({
                            target: '#report-body',
                            message: 'Building PDF Report. <br/>Please wait...',
                            boxed : true
                        });
                    },
                    complete: function () {
                        Utils.unblockUI('#report-body');
                    },
                    url: Config.getApiRootUrl() + '/reports/pdfdownload',
                    type: "POST",
                    dataType: "json",
                    contentType: "application/json; charset=utf-8",
                    data: JSON.stringify(request),
                    success: function (pdfUrl) {
                        if(reportDisplay === 1){
                            $('#pdf-modal').modal({
                                show : true,
                                backdrop: 'static',
                                keyboard: false
                            });
                            $('#pdf-iframe').attr('src', pdfUrl);
                        }else{
                            window.open(pdfUrl, '_blank');
                        }
                    },
                    failure: function (errMsg) {
                        alert(errMsg);
                    }
                });
            }
        },        
        makePrintMode: function () {             
            $('#period-comments-div').removeClass('display-hide');           
            $('#btn-print-pdf').addClass('display-hide');
            let fillerDiv = '<div class="col-md-12 col-sm-12 col-xs-12 fillerDiv" >&nbsp;</div>';
            $('.commenttable').append($(fillerDiv));
            $('.dataTables_wrapper').addClass('display-hide');
            $('.commentlink').addClass('display-hide');
            
            if(!customerExperiencePageBroke)
            {
                $('#customer-experience-page-break').attr("style","page-break-after:always");
            }
        },
        makeScreenMode: function () {
            $('#btn-print-pdf').removeClass('display-hide');
            $("div.fillerDiv").remove()
            $('.dataTables_wrapper').removeClass('display-hide');
            $('.commentlink').removeClass('display-hide');            
            $('#period-comments-div').addClass('display-hide');
        },
        buildPdfHtml: function () {
            let html = '';
            html += '<html lang="en">';
            html += '<head>';
            html += '<link href="http://fonts.googleapis.com/css?family=Open+Sans:400,300,600,700&subset=all" rel="stylesheet" type="text/css" />';
            html += '<link href="http://fonts.googleapis.com/css?family=Comfortaa:400,300" rel="stylesheet" type="text/css" />';
            html += '<link href="' + Config.getAppRootUrl() + '/assets/global/plugins/font-awesome/css/font-awesome.min.css" rel="stylesheet" type="text/css" />';
            html += '<link href="' + Config.getAppRootUrl() + '/assets/global/plugins/simple-line-icons/simple-line-icons.min.css" rel="stylesheet" type="text/css" />';
            html += '<link href="' + Config.getAppRootUrl() + '/assets/global/plugins/bootstrap/css/bootstrap.min.css" rel="stylesheet" type="text/css" />';
            html += '<link href="' + Config.getAppRootUrl() + '/assets/global/plugins/uniform/css/uniform.default.css" rel="stylesheet" type="text/css" />';
            html += '<link href="' + Config.getAppRootUrl() + '/assets/global/plugins/bootstrap-switch/css/bootstrap-switch.min.css" rel="stylesheet" type="text/css" />';
            html += '<link href="' + Config.getAppRootUrl() + '/assets/global/plugins/bootstrap-daterangepicker/daterangepicker.min.css" rel="stylesheet" type="text/css" />';
            html += '<link href="' + Config.getAppRootUrl() + '/assets/global/plugins/bootstrap-datepicker/css/bootstrap-datepicker3.min.css" rel="stylesheet" type="text/css" />';
            html += '<link href="' + Config.getAppRootUrl() + '/assets/global/plugins/bootstrap-timepicker/css/bootstrap-timepicker.min.css" rel="stylesheet" type="text/css" />';
            html += '<link href="' + Config.getAppRootUrl() + '/assets/global/plugins/clockface/css/clockface.css" rel="stylesheet" type="text/css" />';
            html += '<link href="' + Config.getAppRootUrl() + '/assets/global/css/components.min.css" rel="stylesheet" type="text/css"  id="style_components" />';
            html += '<link href="' + Config.getAppRootUrl() + '/assets/global/css/plugins.min.css" rel="stylesheet" type="text/css" />';
            html += '<link href="' + Config.getAppRootUrl() + '/assets/layouts/layout4/css/layout.min.css" rel="stylesheet" type="text/css" />';
            html += '<link href="' + Config.getAppRootUrl() + '/assets/layouts/layout4/css/themes/light.min.css" rel="stylesheet" type="text/css"  id="style_color" />';
            html += '<link href="' + Config.getAppRootUrl() + '/assets/layouts/layout4/css/custom.min.css" rel="stylesheet" type="text/css" />';
            html += '<link href="' + Config.getAppRootUrl() + '/css/halo.css" rel="stylesheet" type="text/css" />';
            html += '<link href="' + Config.getAppRootUrl() + '/css/halo.print.css" rel="stylesheet" type="text/css" />';
            html += '</head>';
            html += '<body class="page-container-bg-solid page-header-fixed page-sidebar-closed-hide-logo" style="background-color: #fff;>';
            let reportHtml = $('#report-body').html().replace(/Show comments for this graph/g, "");
            reportHtml = reportHtml.replace('<div class="col-md-12">', '<div class="col-md-12" style="background-color: #fff;">');
            
            html += reportHtml;
            html += '</body>';
            html += '</html>';
            
            return html;
        }
    };

}();


jQuery(document).ready(function(){
    $('#btn-print-pdf').click(function () {
        ReportsPdfBuilder.makePrintMode();
        let request = { PrintHtml : ReportsPdfBuilder.buildPdfHtml() };
        ReportsPdfBuilder.makeScreenMode();
        ReportsPdfBuilder.init(request);        
    });
});
