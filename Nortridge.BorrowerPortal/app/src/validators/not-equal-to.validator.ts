$.validator.addMethod("notEqualTo", function (value, element, parameters) {
    const relatedProperty = <HTMLInputElement>document.querySelector(`[id$=${parameters.dependentProperty}]`);
    return relatedProperty.value !== value;
});

$.validator.unobtrusive.adapters.add("notEqualTo", [], function (options: any) {
    const dependentProperty = options.element.dataset['dependentProperty'];

    options.rules.notEqualTo = { dependentProperty };
    options.messages["notEqualTo"] = options.message;
});
