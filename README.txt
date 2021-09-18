This solution has been created for registering, update and fetching vehicle details.

Primary Idea was to have VehicleService which allow below actions.
1. Register
2. RecordLocation
3. Get Vehicle
4. Get All Vehicles
5. Get LastNLocation for a vehilce(not implmented yet requires to have vehicle reference in location table).

I have defined this service in differnt library project. This Library project has interface ,model and Data Access Logic.

Data Access has been done using standard Repository design patterns. I have used EF core as ORM framework.

Design
● Security
      - Not taken care yet
● Performance
      - It should be pretty quick since only thing we are doing is just insert/update/add operation in DB.Only point, It could be slow at Database access
        Therefore, I have provided a scaled mode in appsetting.json. In that case, we don't update database immediately rather we do it in asynshorous manner.
● Scalability
        For Huge number of requests, database will become bottlneck. There I wanted to use insert the register/location update request in persistable queue
        i.e. kafka. Since setting it up requires too much work. I have created a pesistConcurrent collection. Tehcnically, it won't be able to persist the request but still to mimic
        the producer/consumer mode. In real system, It has to ba kafka or persistent queue which stores the request even if service goes down. Service Registtarion and location update will be alwasy stored in Kafka queue in fault tolerant mode.
● Maintainability
        Its easily maintable and extendable since we are not hodling and inject the right interface. if we want to change something inject new implentation.
        Change the data model and run migration.
● That your code is tested and ready for production
        I have tested core methods of register and location update
● Documentation: None, unless mathematical algorithms
       
I have also added postman collection to test the API. you just need to set an enviormnet variable after import with value ="http://localhost:5000"
