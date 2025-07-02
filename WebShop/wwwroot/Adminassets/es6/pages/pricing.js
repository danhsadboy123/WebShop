class PagesPricing {

    static init() {

        $('#monthly-btn').on('click', (e) => {
            $('#monthly-view').removeClass('d-none');
            $('#annual-view').addClass('d-none')
            $(e.currentTarget).addClass('IsActivated');
            $('#annual-btn').removeClass('IsActivated');
        })

        $('#annual-btn').on('click', (e) => {
            $('#annual-view').removeClass('d-none');
            $('#monthly-view').addClass('d-none');
            $(e.currentTarget).addClass('IsActivated');
            $('#list-view-btn').removeClass('IsActivated');
        })
    }
}

$(() => { PagesPricing.init(); });

