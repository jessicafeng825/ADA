mergeInto(LibraryManager.library,{
	function startKeepAlive(){
		var websocket = new WebSocket("https://jamesamigo.itch.io/testada");
		setInterval(function() {
			websocket.send("ping");
			debug("keep connect");
		}, 5000);
	}
});