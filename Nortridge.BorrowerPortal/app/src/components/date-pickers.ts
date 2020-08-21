import "tempusdominus-bootstrap-4";
import "jquery-mask-plugin";

export default function init() {
    const dataPickers = $('.js-date-picker');

    dataPickers.each((_, elem) => {
        const $this = $(elem);

        const minDate = $this.data('minDate');
        const maxDate = $this.data('maxDate');

        (<any>$this).datetimepicker({
            format: 'MM/DD/YYYY',
            minDate: minDate !== undefined ? minDate : false,
            maxDate: maxDate !== undefined ? maxDate : false,
            keepInvalid: true
        })
    });

    $('.js-date-input').mask('00/00/0000');
}
