class PagesProfile {

    static init() {

        $('#list-view-btn').on('click', (e) => {
            $('#list-view').removeClass('d-none');
            $('#card-view').addClass('d-none')
            $(e.currentTarget).addClass('IsActivated');
            $('#card-view-btn').removeClass('IsActivated');
        })

        $('#card-view-btn').on('click', (e) => {
            $('#card-view').removeClass('d-none');
            $('#list-view').addClass('d-none');
            $(e.currentTarget).addClass('IsActivated');
            $('#list-view-btn').removeClass('IsActivated');
        })
    }
}

$(() => { PagesProfile.init(); });

