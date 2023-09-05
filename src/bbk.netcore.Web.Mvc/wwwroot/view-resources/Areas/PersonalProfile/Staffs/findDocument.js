(function ($) {
    let documentData = [];
    moment.locale(abp.localization.currentLanguage.name);

    function commendationSelectDecision(documentDetail) {
        $("#DecisionNumber").val(documentDetail.decisionNumber);
    }
     
    function staffPlainningSelectDecision(documentDetail) {
        $("#DecisionNumber").val(documentDetail.decisionNumber);
        $("#IssuedDate").val(moment(documentDetail.issuedDate).format('L'));
    }

    function workingProcessSelectDecision(documentDetail) {
        $("#DecisionNumber").val(documentDetail.decisionNumber);
        $("#IssuedDate").val(moment(documentDetail.issuedDate).format('L'));
    }

    function salaryProcessSelectDecision(documentDetail) {
        $("#DecisionNumber").val(documentDetail.decisionNumber);
        $("#IssuedDate").val(moment(documentDetail.issuedDate).format('L'));
    }

    function recruitmentSelectDecision(documentDetail) {
        $("#RegulationsGuideProbation").val(documentDetail.decisionNumber);
        $("#TimeDecision").val(moment(documentDetail.issuedDate).format('L'));
    }

    function goAbroadSelectDecision(documentDetail) {
        $("#DecisionNumber").val(documentDetail.decisionNumber);
        $("#IssuedDate").val(moment(documentDetail.issuedDate).format('L'));
    }

    async function findDocument(documentCategoryEnum, searchTerm) {
        var _documentAppService = abp.services.app.document;
        var result = await _documentAppService.getDocs({ documentCategoryEnum: documentCategoryEnum, searchTerm: searchTerm });
        return result;
    }

    $('#ModalDocumentFilter .modal-body ul').on('click', 'li', function () {
        if ($(this).hasClass('active')) {
            $(this).removeClass('active');
        }
        else {
            $('#ModalDocumentFilter .modal-body ul li.active').removeClass('active');
            $(this).addClass('active');
        }
    });

    $("#select-decision-button").on('click', function () {

        let documentId = $("#ModalDocumentFilter .modal-body ul li.active").attr("data-id");
        if (!documentId) {
            return;
        }
        var documentDetail = documentData.find((element) => element.id == documentId);
        if (documentDetail) {
            let documentCategoryEnum = $("#Modal .modal-content").attr("document-enum");
            let documentCategoryData = $("#Modal .modal-content").attr("document-data");
            $("#DocumentId").val(documentDetail.id);
            if (documentCategoryEnum && $("#DocumentId").val()) {
                switch (documentCategoryData) {
                    case 'commendation': // commendation
                        commendationSelectDecision(documentDetail);
                        break;
                    case 'staffPlainning': // staff plainning
                        staffPlainningSelectDecision(documentDetail);
                        break;
                    case 'workingProcess':
                        workingProcessSelectDecision(documentDetail);
                        break;
                    case 'salaryProcess':
                        salaryProcessSelectDecision(documentDetail);
                        break;
                    case 'recruitment':
                        recruitmentSelectDecision(documentDetail);
                        break;
                    case 'goAbroad':
                        goAbroadSelectDecision(documentDetail);
                        break;
                    default:
                    // code block
                }
            }
        } else {
            return;
        }
        $("#ModalDocumentFilter").modal('hide');
    });

    $('#ModalDocumentFilter').on('hide.bs.modal', () => {
        $("#ModalDocumentFilter ul").empty();
        $("#ModalDocumentFilter input").val("");
    });

    $(document).on('click', '.select-decision', function () {
        $("#ModalDocumentFilter").modal('show');
    });

    var setTimeoutInputId = null;
    $(document).on('keyup', '#document_list_filter', function (e) {
        if (setTimeoutInputId) {
            clearTimeout(setTimeoutInputId);
        }
        let documentCategoryEnum = $("#Modal .modal-content").attr("document-enum");
        if (!documentCategoryEnum) {
            return;
        }
        var value = $("#document_list_filter").val();
        if (value) {
            setTimeoutInputId = setTimeout(() => {
                findDocument(documentCategoryEnum, value).then(data => {
                    $("#ModalDocumentFilter ul").empty();
                    documentData = data.items;
                    for (let i = 0; i < data.items.length; i++) {
                        var element = `<li class="list-group-item" data-id=${data.items[i].id}><span data-filter-tags="reports file">${data.items[i].description} - ${data.items[i].decisionNumber} - ${moment(data.items[i].issuedDate).format('L')}</span></li>`;
                        $("#ModalDocumentFilter ul").prepend(element);
                    }
                });
            }, 500);
        } else {
            $("#ModalDocumentFilter ul").empty();
            documentData = [];
        }
    });

    var setTimeoutButtonId
    $(document).on('click', '#ModalDocumentFilter #js_default_list .input-group-append', function (e) {
        if (setTimeoutButtonId) {
            clearTimeout(setTimeoutButtonId);
        }
        let documentCategoryEnum = $("#Modal .modal-content").attr("document-enum");
        if (!documentCategoryEnum) {
            return;
        }
        var value = $("#document_list_filter").val();
        if (value) {
            setTimeoutButtonId = setTimeout(() => {
                findDocument(documentCategoryEnum, value).then(data => {
                    $("#ModalDocumentFilter ul").empty();
                    documentData = data.items;
                    for (let i = 0; i < data.items.length; i++) {
                        var element = `<li class="list-group-item" data-id=${data.items[i].id}><span data-filter-tags="reports file">${data.items[i].description} - ${data.items[i].decisionNumber} - ${moment(data.items[i].issuedDate).format('L')}</span></li>`;
                        $("#ModalDocumentFilter ul").prepend(element);
                    }
                });
            }, 500);
        } else {
            $("#ModalDocumentFilter ul").empty();
            documentData = [];
        }
    });
})(jQuery);