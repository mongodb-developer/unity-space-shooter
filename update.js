// Update Document with Realm Function

exports = async function (payload, response) {

    const body = JSON.parse(payload.body.text());

    const result = await context.services
        .get("mongodb-atlas")
        .db("space_shooter")
        .collection("player_profile")
        .updateOne(
            { "_id": body.id },
            { "$set": body.blasters }
        );

    const doc = await context.services
        .get("mongodb-atlas")
        .db("space_shooter")
        .collection("player_profile")
        .findOne({ "_id": body.id });

    return JSON.stringify(doc);

};