// List Entire Collection with Realm Function

exports = async function (payload, response) {

    const docs = await context.services
        .get("mongodb-atlas")
        .db("space_shooter")
        .collection("PlayerProfile")
        .find({})
        .sort({ high_score: -1 })
        .toArray();

    return JSON.stringify(docs);

};