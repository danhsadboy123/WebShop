//$(document).ready(function () {
//    $('.nav-item a').on('click', function (e) {
//        e.preventDefault(); // Ngăn chặn hành vi mặc định của liên kết

//        var url = $(this).attr('href'); // Lấy URL mục tiêu từ liên kết được click

//        $.get(url, function (data) {
//            // Xác định phần nội dung mới từ dữ liệu được trả về
//            var newContent = $(data).find('.main-content').html();

//            // Thay đổi nội dung trong phần main-content
//            $('#mainContent').html(newContent);

//            // Đánh dấu mục điều hướng được click là 'active' (nổi bật)
//            $('.nav-item').removeClass('active');
//            $(this).closest('.nav-item').addClass('active');
//        });
//    });
//});