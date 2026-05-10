
    function carouselInit(id) {
        const el = document.getElementById(id);
        if (!el) return;
        const slides = el.querySelectorAll('.carousel-slide');
        const dots = el.querySelectorAll('.carousel-dot');
        if (slides.length === 0) return;
        el.dataset.index = '0';
        slides[0].classList.remove('opacity-0');
        slides[0].classList.add('opacity-100');
        if (dots.length) { dots[0].classList.remove('bg-white\/60'); dots[0].classList.add('bg-white'); }
    }

    function carouselGo(id, next) {
        const el = document.getElementById(id);
        if (!el) return;
        const slides = el.querySelectorAll('.carousel-slide');
        const dots = el.querySelectorAll('.carousel-dot');
        let idx = parseInt(el.dataset.index) || 0;
        slides[idx].classList.remove('opacity-100');
        slides[idx].classList.add('opacity-0');
        if (dots.length) { dots[idx].classList.remove('bg-white'); dots[idx].classList.add('bg-white\/60'); }
        idx = (idx + next + slides.length) % slides.length;
        el.dataset.index = String(idx);
        slides[idx].classList.remove('opacity-0');
        slides[idx].classList.add('opacity-100');
        if (dots.length) { dots[idx].classList.remove('bg-white\/60'); dots[idx].classList.add('bg-white'); }
    }

    function carouselNext(id) { carouselGo(id, 1); }
    function carouselPrev(id) { carouselGo(id, -1); }
 
    document.addEventListener("DOMContentLoaded", () => {
        document.querySelectorAll('[id^="carousel-"]').forEach(el => carouselInit(el.id)); 
    });