$.validator.addMethod("requiredIfSubmitPaymentMethod", function (value, element, parameters) {
    const relatedProperty = <HTMLInputElement>document.querySelector(`[id^=${parameters.relatedMethod}]:checked`);
    if (relatedProperty === undefined || relatedProperty === null) {
        return true;
    }

    return relatedProperty.value !== parameters.method || (value !== undefined && value !== null && value !== "");
});

$.validator.unobtrusive.adapters.add("requiredIfSubmitPaymentMethod", [], function (options: any) {
    const relatedMethod = options.element.dataset['relatedMethod'];
    const method = options.element.dataset['method'];

    options.rules.requiredIfSubmitPaymentMethod = { relatedMethod, method };
    options.messages["requiredIfSubmitPaymentMethod"] = options.message;
});
