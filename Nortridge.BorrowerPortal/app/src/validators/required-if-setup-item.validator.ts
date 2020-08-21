$.validator.addMethod("requiredIfSetupItem", function (value, element, parameters) {
    const relatedProperty = <HTMLInputElement>document.querySelector(`[id^=${parameters.relatedSetupItem}]:checked`);
    if (relatedProperty.value == parameters.allowEmpty) {
        return true;
    }

    return value !== undefined && value !== null && value !== "";
});

$.validator.unobtrusive.adapters.add("requiredIfSetupItem", [], function (options: any) {
    const relatedSetupItem = options.element.dataset['relatedSetupItem'];
    const allowEmpty = options.element.dataset['allowEmpty'];

    options.rules.requiredIfSetupItem = { relatedSetupItem, allowEmpty };
    options.messages["requiredIfSetupItem"] = options.message;
});
