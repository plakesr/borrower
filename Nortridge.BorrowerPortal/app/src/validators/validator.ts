import './not-equal-to.validator'
import './required-if-setup-item.validator'
import './required-if-submit-payment-method.validator'

import Price from 'shared/price'

$.validator.setDefaults({ ignore: '' });

$.validator.methods.number = function (value: string, element: any) {
    return this.optional(element) || Price.of(value).isNumber();
}

$.validator.methods.range = function (value: string, element: any, param: number[]) {
    const min = param[0];
    const max = param[1];
    const price = Price.of(value).value;

    return this.optional(element) || (min <= price && price <= max);
}
