create table rate_rank
(
    id           int auto_increment
        primary key,
    deposit_rank double not null,
    rate         double not null
);

INSERT INTO integration.rate_rank (deposit_rank, rate) VALUES (200000000, 0.045);
INSERT INTO integration.rate_rank (deposit_rank, rate) VALUES (20000000, 0.035);
INSERT INTO integration.rate_rank (deposit_rank, rate) VALUES (0, 0.03);
INSERT INTO integration.rate_rank (deposit_rank, rate) VALUES (50000000, 0.04);
