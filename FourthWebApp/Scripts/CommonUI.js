


function OpenConfirmWindowModal(url, title, width) {
    if (width == 0) {
        return $.confirm({
            content: 'url:' + url,
            title: title,
            backgroundDismiss: true,
            columnClass: 'col-lg-12',
            containerFluid: true,
            //theme: 'dark',
            closeIcon: true,
            escapeKey: true,
            onClose: function () {

                if (url.includes("Lead/ManageLead"))
                {
                    //used for lead search change + enter
                    isLeadPopupOpened = 0;
                }
                
                if (url.includes("Calendar/AllWOStatusDetails"))
                {
                    woStatusCalendarProjectId = 0;
                    projectCalendarOpenFrom = 0;
                }
            },
            buttons: {

                okButton: {
                    text: 'ok',
                    isHidden: true, // hide the button
                    action: function () {

                    }
                }
            }

        });
    }
    else {
        return $.confirm({
            content: 'url:' + url,
            title: title,
            backgroundDismiss: false,
            closeIcon: true,
            boxWidth: width + 'px',
            useBootstrap: false,
            escapeKey: true,            
            //theme: 'dark',
            onClose: function () {

                if (url.includes("Lead/ManageLead")) {
                    //used for lead search change + enter
                    isLeadPopupOpened = 0;
                }

            },
            buttons: {

                okButton: {
                    text: 'ok',
                    isHidden: true, // hide the button
                    action: function () {

                    }
                }
            }
        });
    }
}

function OpenConfirmWindowModalNoBackgroundDismiss(url, title, width) {

    if (width == 0) {
        return $.confirm({
            content: 'url:' + url,
            title: title,
            backgroundDismiss: false,
            columnClass: 'col-lg-12',
            containerFluid: true,
            //theme: 'dark',
            closeIcon: true,            
            escapeKey: true,
            onClose: function () {

                AllRefresh();

                //used for wo search change + enter
                isPopupOpened_Layout = 0;                

            },
            buttons: {

                okButton: {
                    text: 'ok',
                    isHidden: true, // hide the button
                    action: function () {

                    }
                }
            }

        });
    }
    else {
        return $.confirm({
            content: 'url:' + url,
            title: title,
            backgroundDismiss: false,
            boxWidth: width + 'px',
            useBootstrap: false,
            //columnClass: 'col-lg-12',
            containerFluid: true,
            //theme: 'dark',
            closeIcon: true,
            escapeKey: true,
            onClose: function () {                

                AllRefresh();

                //used for wo search change + enter
                isPopupOpened_Layout = 0;                

            },
            buttons: {

                okButton: {
                    text: 'ok',
                    isHidden: true, // hide the button
                    action: function () {

                    }
                }
            }

        });
    }
    
}

function OpenConfirmWindowModalNoBackgroundDismissFunctionCallOnClose_CS(url, title, width) //cs=> child step
{

    if (width == 0) {
        return $.confirm({
            content: 'url:' + url,
            title: title,
            backgroundDismiss: false,
            columnClass: 'col-lg-12',
            containerFluid: true,
            //theme: 'dark',
            closeIcon: true,
            escapeKey: true,
            onClose: function () {

                AllRefresh();

                //used for wo search change + enter
                isPopupOpened_Layout = 0;

               

            },
            buttons: {

                okButton: {
                    text: 'ok',
                    isHidden: true, // hide the button
                    action: function () {

                    }
                }
            }

        });
    }
    else {
        return $.confirm({
            content: 'url:' + url,
            title: title,
            backgroundDismiss: false,
            boxWidth: width + 'px',
            useBootstrap: false,
            //columnClass: 'col-lg-12',
            containerFluid: true,
            //theme: 'dark',
            closeIcon: true,
            escapeKey: true,
            onClose: function () {
                
                AllRefresh();

                //used for wo search change + enter
                isPopupOpened_Layout = 0;

                

            },
            buttons: {

                okButton: {
                    text: 'ok',
                    isHidden: true, // hide the button
                    action: function () {

                    }
                }
            }

        });
    }

}

function OpenConfirmWindowModalNoBackgroundDismissFunctionCallOnClose(url, title, width) {

    if (width == 0) {
        return $.confirm({
            content: 'url:' + url,
            title: title,
            backgroundDismiss: false,
            columnClass: 'col-lg-12',
            containerFluid: true,
            //theme: 'dark',
            closeIcon: true,
            escapeKey: true,
            onClose: function ()
            {          
                //console.log(projectDetailsPageChange);
                //console.log(showAllFutereStepSaveButtonClicked);
                //console.log(showAllChildStepSaveButtonClicked);
                //console.log(stepNameDirectSave);
                //console.log(KPIPaymentLog);

                AllRefresh();

                //used for project search change + enter
                isPopupOpened = 0;
                isPopupOpened_Layout = 0;                
                
            },
            buttons: {

                okButton: {
                    text: 'ok',
                    isHidden: true, // hide the button
                    action: function () {

                    }
                }
            }

        });
    }
    else {
        return $.confirm({
            content: 'url:' + url,
            title: title,
            backgroundDismiss: false,
            boxWidth: width + 'px',
            useBootstrap: false,
            //columnClass: 'col-lg-12',
            containerFluid: true,
            //theme: 'dark',
            closeIcon: true,
            escapeKey: true,
            buttons: {

                okButton: {
                    text: 'ok',
                    isHidden: true, // hide the button
                    action: function () {

                    }
                }
            }

        });
    }

}

function OpenConfirmWindowModalBackgroundDismiss(url, title, width) {

    return $.confirm({
        content: 'url:' + url,
        title: title,
        backgroundDismiss: true,
        closeIcon: false,
        boxWidth: width + 'px',
        useBootstrap: false,
        escapeKey: true,
        //theme: 'dark',
        buttons: {

            okButton: {
                text: 'ok',
                isHidden: true, // hide the button
                action: function () {

                }
            }
        }
    });
    

}

function OpenConfirmWindowModalWithButton(url, title, width) {
    
   
        return $.confirm({
            content: 'url:' + url,
            title: title,
            backgroundDismiss: true,
            closeIcon: true,
            boxWidth: width + 'px',
            useBootstrap: false,
            escapeKey: true,            
            //theme: 'dark',
            buttons: {

                okButton: {
                    text: 'Submit',
                    //isHidden: true, // hide the button
                    action: function () {
                        return GenerateString();
                    }
                }
            }
        });
    
}

function AllRefresh()
{
    if (projectDetailsPageChange == 1 || showAllFutereStepSaveButtonClicked == 1 || stepDetailsPageChange == 1 || showAllChildStepSaveButtonClicked == 1 || stepNameDirectSave == 1 || KPIPaymentLog == 1 || projectSaveButtonClicked == 1) {

        projectDetailsPageChange = 0;
        stepDetailsPageChange = 0;
        showAllFutereStepSaveButtonClicked = 0;
        showAllChildStepSaveButtonClicked = 0;        
        KPIPaymentLog = 0;
        projectSaveButtonClicked = 0;

        //categoryChange = 0;

        //alert('before the modal is closed');
        //alert("RefreshGrid")        
        
        {
            //OJ
            if (popupOpenFrom == 1) {
                popupOpenFrom = 0;
                RefreshGrid();
            }

            //collection
            if (popupOpenFrom == 2) {

                popupOpenFrom = 0;
                GenerateReport();
            }

            //warrenty
            if (popupOpenFrom == 3) {
                popupOpenFrom = 0;
                RefreshWarrantyGrid();
            }

            //inspection
            if (popupOpenFrom == 4) {
                popupOpenFrom = 0;
                RefreshPendingInspectionGrid();
            }

            //Need scheduling
            if (popupOpenFrom == 5) {
                popupOpenFrom = 0;
                RefreshNeedSchedulingGrid();
            }

            //All Materials
            if (popupOpenFrom == 6) {
                popupOpenFrom = 0;
                LoadAllProjectMaterials();
            }

            //Dashboard
            if (popupOpenFrom == 7) {
                popupOpenFrom = 0;
                Refresh_RTC();
            }

            //All Wo Status calendar
            if (popupOpenFrom == 8) {
                popupOpenFrom = 0;
                LoadWOStatusDetails(-2);
            }
        }


        stepNameDirectSave = 0;
        
    }

}


//function InitiateDatePicker() {
//    // Date Picker
//    jQuery('#datepicker').datepicker();
//    jQuery('.datepicker-autoclose').datepicker({
//        format: "mm/dd/yyyy",
//        autoclose: true,
//        todayHighlight: true
//    });
//    jQuery('#datepicker-inline').datepicker();
//    jQuery('#datepicker-multiple-date').datepicker({
//        format: "mm/dd/yyyy",
//        clearBtn: true,
//        multidate: true,
//        multidateSeparator: ","
//    });
//    jQuery('#date-range').datepicker({
//        toggleActive: true
//    });
//}