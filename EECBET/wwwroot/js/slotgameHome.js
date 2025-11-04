 // .mySwiper 都獨立初始化
        document.querySelectorAll(".mySwiper").forEach((swiperEl, index) => {
            const nextBtn = swiperEl.querySelector(".swiper-button-next");
            const prevBtn = swiperEl.querySelector(".swiper-button-prev");

            const swiper = new Swiper(swiperEl, {
                slidesPerView: 6,
                spaceBetween: 8,
                loop: false, // ❌ 不循環
                speed: 1000, // 平滑
                autoplay: {
                    delay: 0,
                    disableOnInteraction: false
                },
                navigation: {
                    nextEl: nextBtn,
                    prevEl: prevBtn
                },

                // js響應是
                breakpoints: {
                    992: { slidesPerView: 6 },               
                    768: { slidesPerView: 5 },
                    576: { slidesPerView: 4 },
                    0: { slidesPerView: 3 },
                },
            });

            swiper.autoplay.stop(); // 預設靜止

            // 到頭或到尾 → 停止
            swiper.on("slideChange", () => {
                if (swiper.isBeginning || swiper.isEnd) {
                    swiper.autoplay.stop();
                }
            });

            // Hover 右箭頭 → 往後播
            nextBtn.addEventListener("mouseenter", () => {
                if (!swiper.isEnd) {
                    swiper.params.autoplay.reverseDirection = false;
                    swiper.autoplay.start();
                }
            });
            nextBtn.addEventListener("mouseleave", () => swiper.autoplay.stop());

            // Hover 左箭頭 → 往前播
            prevBtn.addEventListener("mouseenter", () => {
                if (!swiper.isBeginning) {
                    swiper.params.autoplay.reverseDirection = true;
                    swiper.autoplay.start();
                }
            });
            prevBtn.addEventListener("mouseleave", () => swiper.autoplay.stop());
        });