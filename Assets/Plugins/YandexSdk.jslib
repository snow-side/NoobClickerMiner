mergeInto(LibraryManager.library, {
  InitSDK: function (photoSize, scopes) {
    InitSDK(UTF8ToString(photoSize), UTF8ToString(scopes));
  },
  FullAdShow: function () {
    FullAdShow();
  },
  RewardedShow: function (val) {
    RewardedShow(UTF8ToString(val));
  },
  SaveLocalStorage: function (key, json) {
    SaveLocalStorage(UTF8ToString(key), UTF8ToString(json));
  },
  LoadLocalStorage: function (key) {
    LoadLocalStorage(UTF8ToString(key));
  },
  MetricaGoal: function (name, value) {
    MetricaGoal(UTF8ToString(name), UTF8ToString(value));
  },
});
