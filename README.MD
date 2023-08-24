# API OrderEase

## EF Core

### Migrations

#### Add

```bash
dotnet ef migrations add <NAME> -p Infrastructure/Infrastructure.csproj
```
#### Remove

```bash
dotnet ef migrations remove -p Infrastructure/Infrastructure.csproj
```

#### Database

```bash
dotnet ef database update -p Infrastructure/Infrastructure.csproj

dotnet ef database update <NAME> -p Infrastructure/Infrastructure.csproj
```

### Diagrama MER https://dbdiagram.io/d

```bash
// Use DBML to define your database structure
// Docs: https://dbml.dbdiagram.io/docs

Table users {
  id int [primary key]
  email varchar
  password varchar
  name varchar
  birth_date timestamp
  roles_id int
  active bit
  updated_at timestamp
  created_at timestamp
}

Table roles {
  id int [primary key]
  name varchar
  order_access tinyint
  product_access tinyint
  user_access tinyint
  created_at timestamp
}

Ref: roles.id < users.roles_id

Table products {
  id int [primary key]
  name varchar
  description varchar
  unit varchar
  weight decimal
  height decimal
  width decimal
  length decimal
  stock decimal
  categories_id int
  active bit
  updated_at timestamp
  created_at timestamp
}

Ref: products.id < product_images.products_id
Ref: products.id < product_prices.products_id

Table categories {
  id int [primary key]
  name varchar
  created_at timestamp
}

Ref: categories.id < products.categories_id

Table product_images {
  id int [primary key]
  name varchar
  archive varchar
  products_id int
  created_at timestamp
}

Table product_prices {
  id int [primary key]
  price decimal
  price_lists_id int
  products_id int
  created_at timestamp
}

Ref: price_lists.id < product_prices.price_lists_id

Table price_lists {
  id int [primary key]
  name varchar
  active bit
  created_at timestamp
}

Table customers {
  id int [primary key]
  name varchar
  email varchar
  fone varchar
  birth_date timestamp
  active bit
  updated_at timestamp
  created_at timestamp
}

Ref: customers.id < customer_addresses.customers_id 

Table customer_addresses {
  id int [primary key]
  zip_code varchar
  city varchar
  state varchar
  neighborhood varchar
  complement varchar
  street varchar
  customers_id int
  created_at timestamp
}

Ref: users.id < orders.users_id
Ref: customers.id < orders.customers_id
Ref: customer_addresses.id < orders.customer_addresses_id

Table orders {
  id int [primary key]
  total decimal
  freight decimal
  users_id int
  status tinyint
  customers_id int
  customer_addresses_id int
  updated_at timestamp
  created_at timestamp
}

Ref: orders.id < order_items.orders_id
Ref: products.id < order_items.products_id
Ref: price_lists.id < order_items.price_lists_id

Table order_items {
  id int [primary key]
  total decimal
  orders_id int
  products_id int
  price_lists_id int
  quantity decimal
  price decimal
  discount decimal
  addition decimal
  updated_at timestamp
  created_at timestamp
}
```
