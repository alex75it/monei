"use strict";
    
describe("UtilsProvider", function() {
    var utilsProvider, httpBackend;
    var appName = app.name; // this "initialize" module, all other way give an error
    
    beforeEach(angular.mock.module("monei"));

    //var $injector = angular.injector(["monei"]);

    it("inject \"utils\" should contains UtilsService", function () {
        window.inject(function(utils) {
            expect(utils).not.toEqual(null);
        });
    });

    it("when call toShortDate() with null should return empty string", function () {
        inject(function (utils) {
            var date = null;
            expect(utils.toShortDate()).toEqual("");
        });
    });

    it("when call toShortDate() with moment() object should return shortDate format date", function () {
        inject(function (utils) {
            var date = moment();
            var result = utils.toShortDate(date);
            expect(result).not.toEqual(null);
            expect(result).not.toEqual("");
            // todo... how to test culture dipendent format?
        });
    });

    it("when call getDate() with moment() object should return not null or empty", function() {
        inject(function(utils) {
            var date = moment();
            var result = utils.getDate(date);
            expect(result).not.toEqual(null);
            expect(result).not.toEqual("");
        });
    });

    it("when call getDate() with Date() object should return not null or empty", function () {
        inject(function (utils) {
            var date = Date();
            var result = utils.getDate(date);
            expect(result).not.toEqual(null);
            expect(result).not.toEqual("");
        });
    });

});


/*
describe("Player", function () {
  var player;
  var song;

  beforeEach(function() {
    player = new Player();
    song = new Song();
  });

  it("should be able to play a Song", function() {
    player.play(song);
    expect(player.currentlyPlayingSong).toEqual(song);

    //demonstrates use of custom matcher
    expect(player).toBePlaying(song);
  });

  describe("when song has been paused", function() {
    beforeEach(function() {
      player.play(song);
      player.pause();
    });

    it("should indicate that the song is currently paused", function() {
      expect(player.isPlaying).toBeFalsy();

      // demonstrates use of 'not' with a custom matcher
      expect(player).not.toBePlaying(song);
    });

    it("should be possible to resume", function() {
      player.resume();
      expect(player.isPlaying).toBeTruthy();
      expect(player.currentlyPlayingSong).toEqual(song);
    });
  });

  // demonstrates use of spies to intercept and test method calls
  it("tells the current song if the user has made it a favorite", function() {
    spyOn(song, 'persistFavoriteStatus');

    player.play(song);
    player.makeFavorite();

    expect(song.persistFavoriteStatus).toHaveBeenCalledWith(true);
  });

  //demonstrates use of expected exceptions
  describe("#resume", function() {
    it("should throw an exception if song is already playing", function() {
      player.play(song);

      expect(function() {
        player.resume();
      }).toThrowError("song is already playing");
    });
  });
});

*/