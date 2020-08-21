export default function init() {
    const webpay = document.getElementById('web-pay');
    if (webpay) {
        const url = webpay.dataset['url'];
        window.open(url, '_new');
        window.focus();
    }
}
