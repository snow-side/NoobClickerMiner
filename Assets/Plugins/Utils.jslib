mergeInto(LibraryManager.library, {
  IsMobile: function () {
    return "ontouchstart" in window;
  },
});
