/**
 * Mobile menu functionality
 */
function initMobileMenu() {
  const menuBtn = document.querySelector(
    ".new_header__second_row__mobile_group__menu_button"
  );
  const mobileMenu = document.getElementById("mobileMenu");
  const closeMenuBtn = document.getElementById("closeMobileMenu");
  const orderProductLabelInMenu = document.querySelector(
    "#mobileMenu .mobile-menu__order-btn"
  );

  if (menuBtn && mobileMenu && closeMenuBtn) {
    menuBtn.addEventListener("click", () => {
      mobileMenu.style.display = "flex";
      document.body.style.overflow = "hidden";
    });

    closeMenuBtn.addEventListener("click", () => {
      mobileMenu.style.display = "none";
      document.body.style.overflow = "";
    });

    if (orderProductLabelInMenu) {
      orderProductLabelInMenu.addEventListener("click", () => {
        mobileMenu.style.display = "none";
        document.body.style.overflow = "";
      });
    }
  }
}
/**
 * Initialize all components
 */
function initComponents() {
  initMobileMenu();
}

document.addEventListener("DOMContentLoaded", initComponents);

document.addEventListener("DOMContentLoaded", () => {
  const accordionItems = document.querySelectorAll(".accordion-item");

  accordionItems.forEach((item) => {
    const header = item.querySelector(".accordion-header");
    const content = item.querySelector(".accordion-content");

    if (header && content) {
      header.addEventListener("click", () => {
        item.classList.toggle("active");

        if (item.classList.contains("active")) {
          content.style.maxHeight = content.scrollHeight + "px";
        } else {
          content.style.maxHeight = null;
        }
      });
    }
  });
});
