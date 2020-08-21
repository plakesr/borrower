export default function init() {
    $('.js-radio-toggler').on('change', function (e) {
        const $this = $(this);

        hideRelatedItems($this);

        const $target = $($this.data('target'));
        $target.removeAttr('hidden');

        const $hide = $($this.data('hide'));
        $hide.attr('hidden', 'hidden');
    });
}

function hideRelatedItems($current: JQuery<HTMLElement>) {
    const name = $current.attr('name');

    const items = $(`.js-radio-toggler[name='${name}']`);
    items.each(function () {
        const $item = $(this);
        const target = $item.data('target');
        if (target) {
            const $target = $(target);
            $target.attr('hidden', 'hidden');
        }
    })
}