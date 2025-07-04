document.addEventListener('DOMContentLoaded', function () {
    const options = document.querySelectorAll('.country_option .option');
    const selectedCategory = document.getElementById('selectedCategory');
    const currentSpan = document.querySelector('.country_option .current');

    options.forEach(option => {
        option.addEventListener('click', function () {
            // Xóa class 'selected' từ tất cả các option
            options.forEach(opt => opt.classList.remove('selected'));
            // Thêm class 'selected' cho option được chọn
            this.classList.add('selected');

            // Cập nhật giá trị hidden input
            selectedCategory.value = this.getAttribute('data-value');
            currentSpan.textContent = this.textContent;

            // Chuyển hướng nếu có slug
            const slug = this.getAttribute('data-value');
            if (slug) {
                window.location.href = `/Product/ProductList?categories=${encodeURIComponent(slug)}`;
            } else {
                window.location.href = `/Product/ProductList`; // Quay về danh sách mặc định
            }
        });
    });
});