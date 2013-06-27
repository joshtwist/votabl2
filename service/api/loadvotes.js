exports.post = function(request, response) {
    var sql = "SELECT COUNT(id) as 'total', votablId FROM votes WHERE eventShareId = ? GROUP BY votablId";
    request.service.mssql.query(sql,[request.query.eventShareId], {
        success:function (results) {
         response.send(200, results);
        }
    })
};