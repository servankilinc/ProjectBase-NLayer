﻿const RequestManager = {
    baseUrl: '', // https://localhost:7058


    Get: function ({
        path = '',
        dataType = 'json',
        requestData = null,
        formId = null,
        buttonId = null,
        buttonElement = null,
        onBefore = null,
        onSuccess = null,
        onAfter = null,
        onError = null,
        waitToastr = false,
        showToastrSuccess = true,
        showToastrError = true,
        successMessage = null,
        errorMessage = null
    }) {

        return this.HandleRequest({
            type: 'GET',
            path: path,
            dataType: dataType,
            requestData: requestData,
            formId: formId,
            buttonId: buttonId,
            buttonElement: buttonElement,
            onBefore: onBefore,
            onSuccess: onSuccess,
            onAfter: onAfter,
            onError: onError,
            waitToastr: waitToastr,
            showToastrSuccess: showToastrSuccess,
            showToastrError: showToastrError,
            successMessage: successMessage,
            errorMessage: errorMessage,
        })
    },
    Delete: function ({
        path = '',
        dataType = 'json',
        requestData = null,
        formId = null,
        buttonId = null,
        buttonElement = null,
        onBefore = null,
        onSuccess = null,
        onAfter = null,
        onError = null,
        waitToastr = false,
        showToastrSuccess = true,
        showToastrError = true,
        successMessage = null,
        errorMessage = null
    }) {

        return this.HandleRequest({
            type: 'DELETE',
            path: path,
            dataType: dataType,
            requestData: requestData,
            formId: formId,
            buttonId: buttonId,
            buttonElement: buttonElement,
            onBefore: onBefore,
            onSuccess: onSuccess,
            onAfter: onAfter,
            onError: onError,
            waitToastr: waitToastr,
            showToastrSuccess: showToastrSuccess,
            showToastrError: showToastrError,
            successMessage: successMessage,
            errorMessage: errorMessage,
        })
    },

    Post: function ({
        path = '',
        dataType = 'json',
        requestData = null,
        formId = null,
        buttonId = null,
        buttonElement = null,
        onBefore = null,
        onSuccess = null,
        onAfter = null,
        onError = null,
        waitToastr = false,
        showToastrSuccess = true,
        showToastrError = true,
        successMessage = null,
        errorMessage = null
    }) {
        return this.HandleRequest({
            type: 'POST',
            path: path,
            dataType: dataType,
            requestData: requestData,
            formId: formId,
            buttonId: buttonId,
            buttonElement: buttonElement,
            onBefore: onBefore,
            onSuccess: onSuccess,
            onAfter: onAfter,
            onError: onError,
            waitToastr: waitToastr,
            showToastrSuccess: showToastrSuccess,
            showToastrError: showToastrError,
            successMessage: successMessage,
            errorMessage: errorMessage,
        })
    },

    HandleRequest: function ({
        type = 'GET',
        path = '',
        dataType = 'json',
        requestData = null,
        formId = null,
        buttonId = null,
        buttonElement = null,
        onBefore = null,
        onSuccess = null,
        onAfter = null,
        onError = null,
        waitToastr = false,
        showToastrSuccess = true,
        showToastrError = true,
        successMessage = null,
        errorMessage = null
    }) {

        let _baseUrl = this.baseUrl;
        return new Promise(function (resolve, reject) {
            const data = {};

            if (requestData != null && Array.isArray(requestData)) // If Request Data is Array of serialized form
            {
                const formDataObj = {};
                requestData.forEach(item => {
                    formDataObj[item.name] = item.value;
                });
                Object.assign(data, formDataObj);
            }
            else if (requestData != null && typeof requestData === 'object') // If Request Data is object
            {
                Object.assign(data, requestData);
            }

            if (formId != null) {
                const formDataArray = $(`#${formId}`).serializeArray();
                const formDataObj = {};
                formDataArray.forEach(item => {
                    formDataObj[item.name] = item.value;
                });
                Object.assign(data, formDataObj);
            }

            let originalBtnContent = '...';
            if (path[0] != '/') path = '/' + path.trim();

            $.ajax({
                url: `${_baseUrl}${path}`,
                type: type,
                dataType: dataType,
                data: data,
                beforeSend: function () {
                    if (buttonId != null) {
                        let btn = $(`#${buttonId}`);
                        btn.prop("disabled", true);
                        let dynamicContent = btn.find(".dynamic-content");
                        if (dynamicContent.length == 1) {
                            originalBtnContent = dynamicContent.html();
                            dynamicContent.html('<i class="fa-solid fa-spinner me-2"></i> Bekleyiniz');
                        }
                    }
                    else if (buttonElement != null) {
                        let btn = $(buttonElement);
                        btn.prop("disabled", true);
                        let dynamicContent = btn.find(".dynamic-content");
                        if (dynamicContent.length == 1) {
                            originalBtnContent = dynamicContent.html();
                            dynamicContent.html('<i class="fa-solid fa-spinner me-2"></i> Bekleyiniz');
                        }
                    }

                    if (onBefore != null && typeof onBefore === 'function') onBefore();
                },
                success: function (response) {
                    const isThereOnSuccess = onSuccess != null && typeof onSuccess === 'function';
                    if (showToastrSuccess) {
                        if (waitToastr) {
                            AlertManager.Success(successMessage).then(() => { if (isThereOnSuccess) onSuccess(response) })
                        }
                        else {
                            AlertManager.Success(successMessage);
                            if (isThereOnSuccess) onSuccess(response)
                        }
                    }
                    else if (isThereOnSuccess) onSuccess(response);

                    resolve(response);
                },
                error: function (xhr, status, error) {

                    if (xhr.responseJSON != null && xhr.responseJSON.type != null) {
                        if (xhr.responseJSON.type == "Business") {
                            if (errorMessage == null) {
                                errorMessage = xhr.responseJSON.Detail;
                            }
                        }
                        else if (xhr.responseJSON.type == "Validation") {
                            if (errorMessage == null) {
                                errorMessage = "Eksik veya Hatalı Bilgi Gönderildiği İçin İşleminiz Devam edemiyoruz.";
                            }
                            ShowValidationErrors(xhr.responseJSON);
                        }
                    }

                    const isThereOnError = onError != null && typeof onError === 'function';
                    if (showToastrError) {
                        if (waitToastr) {
                            AlertManager.Error(errorMessage)
                                .then(() => {
                                    if (isThereOnError) onError(error);
                                });
                        }
                        else {
                            AlertManager.Error(errorMessage);
                            if (isThereOnError) onError(error);
                        }
                    }
                    else if (isThereOnError) onError(error);

                    reject(error)
                },
                complete: function () {
                    if (buttonId != null) {
                        var btn = $(`#${buttonId}`);
                        btn.prop("disabled", false);
                        let dynamicContent = btn.find(".dynamic-content");
                        if (dynamicContent.length == 1) {
                            dynamicContent.html(originalBtnContent);
                        }
                    }
                    else if (buttonElement != null) {
                        let btn = $(buttonElement);
                        btn.prop("disabled", false);
                        let dynamicContent = btn.find(".dynamic-content");
                        if (dynamicContent.length == 1) {
                            dynamicContent.html(originalBtnContent);
                        }
                    }

                    if (onAfter != null && typeof onAfter === 'function') onAfter();
                }
            });
        });
    }
}
function ShowValidationErrors(response) {
    var elementsOfValidationMsg = $(`[data-valmsg-for]`);
    if (elementsOfValidationMsg != null && elementsOfValidationMsg.length > 0) {
        elementsOfValidationMsg.each((i, e) => $(e).text(""));
    }

    if (response != null && response.errors != null && Array.isArray(response.errors)) {

        response.errors.forEach((validError, index) => {
            let $target = $(`[data-valmsg-for$='.${validError.propertyName}']`);

            if ($target.length === 0) {

                $target = $(`[data-valmsg-for='${validError.propertyName}']`);
            }

            if ($target.length != 0) {
                $target.text(validError.errorMessage);
            }
        })
    }
}
