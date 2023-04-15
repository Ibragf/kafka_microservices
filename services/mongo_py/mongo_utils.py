from unicodedata import name
import pymongo

def get_mongo():
    return pymongo.MongoClient('mongo', 27017, directConnection=True)

def get_all(mongo):
    res = mongo.find({})

    response = []
    for inst in res:
        del inst["_id"]
        response.append(inst)
    return response

def get_specialitites(mongo, specId):
    res = mongo.find({"departments.specs.id": specId})

    response = []
    for inst in res:
        del inst["_id"]
        response.append(inst)
    return response

def get_courses(mongo, courseId):
    res = mongo.find({"departments.courses.id": courseId})

    response = []
    for inst in res:
        del inst["_id"]
        response.append(inst)
    return response

def get_departments(mongo, depId):
    res = mongo.find({"departments.id": depId})

    response = []
    for inst in res:
        del inst["_id"]
        response.append(inst)
    return response

def create_courses(mongo, name, courseId, depFk):
    mongo.update_one({"departments.id": depFk}, 
                    {"$push": {"departments.$[dep].courses": {"id": courseId, "name": name}}}, 
                    array_filters= [ {"dep.id": depFk}])

def delete_courses(mongo, courseId):
    return mongo.update_one({"departments.courses.id": courseId},
                {"$pull": {"departments.$.courses": {"id": courseId}}})

def create_specs(mongo, specId, name, depFk):
    mongo.update_one({"departments.id": depFk}, 
                    {"$push": {"departments.$[dep].specs": {"id": specId, "name": name}}}, 
                    array_filters= [ {"dep.id": depFk}])

def delete_specs(mongo, specId):
    mongo.update_one({"departments.specs.id": specId}, 
                {"$pull": {"departments.$.specs": {"id": specId}}})

def create_departments(mongo, depId, name, instFk):
    mongo.update_one({"id": instFk}, 
                    {"$push": {"departments": {"id": depId, "name": name}}})

def delete_departments(mongo, depId):
    mongo.update_one({"departments.id": depId}, 
                {"$pull": {"departments": {"id": depId}}})