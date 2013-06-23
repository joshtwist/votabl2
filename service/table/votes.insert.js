function insert(item, user, request) {
	request.execute({
		success: function() {
			mssql.query('SELECT DISTINCT channelUri FROM channels', [], {
				success : function(results) {
					request.respond();
					results.forEach(function(r) {
						push.wns.sendRaw(r.channelUri, JSON.stringify(item));
					});
				}
			});
		}
	});
}