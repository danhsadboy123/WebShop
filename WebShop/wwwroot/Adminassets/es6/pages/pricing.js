class PagesPricing {

    static init() {

        $('#monthly-btn').on('click', (e) => {
            $('#monthly-view').removeClass('d-none');
            $('#annual-view').addClass('d-none')
            $(e.currentTarget).addClass('KichHoat');
            $('#annual-btn').removeClass('KichHoat');
        })

        $('#annual-btn').on('click', (e) => {
            $('#annual-view').removeClass('d-none');
            $('#monthly-view').addClass('d-none');
            $(e.currentTarget).addClass('KichHoat');
            $('#list-view-btn').removeClass('KichHoat');
        })
    }
}

$(() => { PagesPricing.init(); });

