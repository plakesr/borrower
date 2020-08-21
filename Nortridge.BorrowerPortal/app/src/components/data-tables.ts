import "datatables.net-bs4";

export default function init() {
    $('.js-dt-table').DataTable({
        "searching": false,
        "paging": false,
        "info": false,
        "ordering": false,
        "autoWidth": false
    });
}
