	Copyright(c) 2021-2030, Muhammad Rahman(masras2006@gmail.com)
	All rights reserved.

	This project is licensed under the Apache 2.0 License found in the
	LICENSE file in the root directory of this source tree. 


Assumption
=============================
	Assuming there are two pricing source(NASDAQ,NYSE).
	AND in memory Price provider would be used. and this single provider handles both pricing source.
	Ticker could belong to any source.
	

Prerequisite
====================
requires .net 4.5.2 or newer(any compatible .net framework) to run application.

Design
================================
there are three layers provided:

	1. Winform App
	2. Price Engine (keep track of all customers(GUI), source and pricing)
	3. Price provider(Fake) (keep track of engines)


How to USE
=================================

	a)Start Engine
		1. press start button.
		2. all controls would be enabled.

	b)Dispaly Realtime ticker Prices:
		1. select from source dropdown  and then select symbol from dropdown
		2. Press Subscribe Button
		3. Gridview would be refresh with realtime price changes

	c)Unsubscribe Ticker:
		1.just select ticker from dropdowns ,
		2. presss unsibscribe button.
		3. you will see no longe price update

	d)Stop engine:
		1. press stop button
		2. all pricing would stop refresing.


	Copyright(c) 2021-2030, Muhammad Rahman(masras2006@gmail.com)
	All rights reserved.

	This project is licensed under the Apache 2.0 License found in the
	LICENSE file in the root directory of this source tree. 
