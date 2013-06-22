exports.post = function(request, response) {
    var sql = "SELECT COUNT(id) AS 'total', votablId FROM votabl2.votes WHERE eventShareId = ? GROUP BY votablId"
    request.service.mssql.query(query, [request.parameters.eventId], {
        success: function (results) {
            response.send(200, results);
        }
    });
};