class PagesProfile {

    static init() {

        $('#list-view-btn').on('click', (e) => {
            $('#list-view').removeClass('d-none');
            $('#card-view').addClass('d-none')
            $(e.currentTarget).addClass('KichHoat');
            $('#card-view-btn').removeClass('KichHoat');
        })

        $('#card-view-btn').on('click', (e) => {
            $('#card-view').removeClass('d-none');
            $('#list-view').addClass('d-none');
            $(e.currentTarget).addClass('KichHoat');
            $('#list-view-btn').removeClass('KichHoat');
        })
    }
}

$(() => { PagesProfile.init(); });

