exports.get = function(request, response) {
	var votabls = request.service.tables.getTable('votabls');
	votabls.where({eventShareId: request.query.eventShareId}).read({
		success: function(results) {
			results.forEach(function(r) {
				delete r.eventShareId;
			});
			response.send(200, results);
		}
	});
}