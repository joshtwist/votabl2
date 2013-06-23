exports.post = function(request, response) {
    var sql = "SELECT COUNT(id) AS 'total', votablId FROM votabl2.votes WHERE eventShareId = ? GROUP BY votablId"
    request.service.mssql.query(sql, [request.parameters.eventShareId], {
        success: function (results) {
            response.send(200, results);
        }
    });
};