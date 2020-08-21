export default function init() {
    $('form').on('submit', function (ev) {
        const $this = $(this);

        if ($this.valid()) {

            // console.log("submit initiated");

            if ($this.is(':disabled')) {
                // console.log("submit rejected");
                ev.preventDefault();
                return;
            }

            // console.log("submit approved");

            const $input = $this.find('input[type=submit], button[type=submit]');
            $input.attr('disabled', 'disabled');
        }
    });
}
