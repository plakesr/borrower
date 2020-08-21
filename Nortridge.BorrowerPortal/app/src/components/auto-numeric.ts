import AutoNumeric from 'autonumeric';

export default function init() {
    document
        .querySelectorAll('.js-price-input')
        .forEach((elem: HTMLElement) => {
            new AutoNumeric(elem, { selectOnFocus: false });
        });

    document
        .querySelectorAll('.js-price-input-long')
        .forEach((elem: HTMLElement) => {
            new AutoNumeric(elem, { decimalPlaces: 5, selectOnFocus: false });
        });
}
