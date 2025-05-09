/**
 * Initialize filter accordions
 */
function initFilterAccordions() {
  const filterGroups = document.querySelectorAll(
    ".filters-desktop .filter-group"
  );

  filterGroups.forEach((group, index) => {
    const header = group.querySelector(".filter-group__header");
    const content = group.querySelector(".filter-group__content");

    if (header && content) {
      const shouldBeOpenByDefault = true;

      if (shouldBeOpenByDefault) {
        content.style.display = "block";
        header.classList.add("is-open");
      } else {
        content.style.display = "none";
        header.classList.remove("is-open");
      }

      header.addEventListener("click", () => {
        const isOpen = content.style.display === "block";
        if (isOpen) {
          content.style.display = "none";
          header.classList.remove("is-open");
        } else {
          content.style.display = "block";
          header.classList.add("is-open");
        }
      });
    }
  });
}

/**
 * Initialize price range slider
 */
function initPriceRangeSlider() {
  const minPriceInput = document.getElementById("minPriceInput");
  const maxPriceInput = document.getElementById("maxPriceInput");
  const minThumb = document.getElementById("minThumb");
  const maxThumb = document.getElementById("maxThumb");
  const rangeVisual = document.querySelector(".filter-price__slider-range");
  const sliderTrack = document.querySelector(".filter-price__slider-track");
  const sliderElement = document.querySelector(".filter-price__slider");

  if (
    !minPriceInput ||
    !maxPriceInput ||
    !minThumb ||
    !maxThumb ||
    !rangeVisual ||
    !sliderTrack ||
    !sliderElement
  ) {
    console.warn(
      "Price range slider elements not found. Skipping initialization."
    );
    return;
  }

  const SLIDER_MIN_VAL =
    parseInt(sliderElement.dataset.sliderMinDefault, 10) || 0;
  const SLIDER_MAX_VAL =
    parseInt(sliderElement.dataset.sliderMaxDefault, 10) || 5000;
  const MIN_THUMB_SEPARATION = 1;

  function updateSliderFromInputs() {
    let minVal = parseInt(minPriceInput.value, 10);
    let maxVal = parseInt(maxPriceInput.value, 10);

    if (isNaN(minVal)) minVal = SLIDER_MIN_VAL;
    if (isNaN(maxVal)) maxVal = SLIDER_MIN_VAL;

    minVal = Math.max(SLIDER_MIN_VAL, Math.min(SLIDER_MAX_VAL, minVal));
    maxVal = Math.max(SLIDER_MIN_VAL, Math.min(SLIDER_MAX_VAL, maxVal));

    if (minVal > maxVal) {
      minVal = maxVal;
    }

    minPriceInput.value = minVal;
    maxPriceInput.value = maxVal;

    let minPercent =
      ((minVal - SLIDER_MIN_VAL) / (SLIDER_MAX_VAL - SLIDER_MIN_VAL)) * 100;
    let maxPercent =
      ((maxVal - SLIDER_MIN_VAL) / (SLIDER_MAX_VAL - SLIDER_MIN_VAL)) * 100;

    minPercent = Math.max(0, Math.min(100, minPercent));
    maxPercent = Math.max(0, Math.min(100, maxPercent));

    if (
      maxPercent < minPercent + MIN_THUMB_SEPARATION &&
      minPercent > MIN_THUMB_SEPARATION
    ) {
      maxPercent = minPercent + MIN_THUMB_SEPARATION;
      if (maxPercent > 100) {
        maxPercent = 100;
        minPercent = maxPercent - MIN_THUMB_SEPARATION;
      }
    } else if (
      maxPercent < minPercent &&
      maxPercent < 100 - MIN_THUMB_SEPARATION
    ) {
      minPercent = maxPercent - MIN_THUMB_SEPARATION;
      if (minPercent < 0) {
        minPercent = 0;
        maxPercent = minPercent + MIN_THUMB_SEPARATION;
      }
    }

    minThumb.style.left = `${minPercent}%`;
    maxThumb.style.left = `${maxPercent}%`;
    rangeVisual.style.left = `${minPercent}%`;
    rangeVisual.style.width = `${maxPercent - minPercent}%`;
  }

  function updateInputsFromSlider() {
    const minPercent = parseFloat(minThumb.style.left) || 0;
    const maxPercent = parseFloat(maxThumb.style.left) || 0;

    let minVal =
      SLIDER_MIN_VAL + (minPercent / 100) * (SLIDER_MAX_VAL - SLIDER_MIN_VAL);
    let maxVal =
      SLIDER_MIN_VAL + (maxPercent / 100) * (SLIDER_MAX_VAL - SLIDER_MIN_VAL);

    minPriceInput.value = Math.round(minVal);
    maxPriceInput.value = Math.round(maxVal);

    rangeVisual.style.left = `${minPercent}%`;
    rangeVisual.style.width = `${maxPercent - minPercent}%`;
  }

  minPriceInput.addEventListener("input", updateSliderFromInputs);
  maxPriceInput.addEventListener("input", updateSliderFromInputs);

  updateSliderFromInputs();

  function setupThumbDragging(thumbElement) {
    thumbElement.addEventListener("mousedown", (e) => {
      e.preventDefault();
      const trackRect = sliderTrack.getBoundingClientRect();
      const trackWidth = trackRect.width;
      const isMinThumb = thumbElement === minThumb;

      document.body.style.userSelect = "none";

      function onMouseMove(moveEvent) {
        let newX = moveEvent.clientX - trackRect.left;
        let percent = (newX / trackWidth) * 100;

        percent = Math.max(0, Math.min(100, percent));

        if (isMinThumb) {
          const maxThumbPercent = parseFloat(maxThumb.style.left) || 100;
          if (percent >= maxThumbPercent - MIN_THUMB_SEPARATION) {
            percent = maxThumbPercent - MIN_THUMB_SEPARATION;
          }
        } else {
          const minThumbPercent = parseFloat(minThumb.style.left) || 0;
          if (percent <= minThumbPercent + MIN_THUMB_SEPARATION) {
            percent = minThumbPercent + MIN_THUMB_SEPARATION;
          }
        }

        percent = Math.max(0, Math.min(100, percent));

        thumbElement.style.left = `${percent}%`;
        updateInputsFromSlider();
        updateSliderFromInputs();
      }

      function onMouseUp() {
        document.removeEventListener("mousemove", onMouseMove);
        document.removeEventListener("mouseup", onMouseUp);
        document.body.style.userSelect = "";
      }

      document.addEventListener("mousemove", onMouseMove);
      document.addEventListener("mouseup", onMouseUp);
    });
  }

  setupThumbDragging(minThumb);
  setupThumbDragging(maxThumb);
}

function initComponents() {
  initFilterAccordions();
  initPriceRangeSlider();
}

document.addEventListener("DOMContentLoaded", initComponents);
