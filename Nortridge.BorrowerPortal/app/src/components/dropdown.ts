export default function init() {
    $('.js-select-links').on('change', function (e) {
        e.preventDefault();

        const $this = $(this);

        const $holder = $($this.data('holder'));
        const $body = $holder.find('.js-select-body');
        $body.attr('hidden', 'hidden');

        const $option = $this.find('option:selected');
        const target = $option.data('target');
        $(target).removeAttr('hidden');
    });
}
