export default function init() {
    const MILLISECONDS_SHOW_ALERT = 5_000;
    const MILLISECONDS_HIDE_DURATION = 1_000;

    const $target = $('.js-alert-message');
    $target.show().delay(MILLISECONDS_SHOW_ALERT).hide(MILLISECONDS_HIDE_DURATION);
}

