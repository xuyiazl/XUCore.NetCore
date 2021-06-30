
-- ----------------------------
-- Records of sys_admin_authmenus
-- ----------------------------
INSERT INTO `sys_admin_authmenus` VALUES (1, 0, '系统设置', 'fa fa-cogs text-danger', '#', 'sys', b'1', 99, b'0', 1, '2021-03-22 14:25:16', NULL, NULL);
INSERT INTO `sys_admin_authmenus` VALUES (2, 1, '导航设置', 'fa fa-chain text-primary', '/admin/sys/admin/menu/list', 'sys-menus', b'1', 99, b'0', 1, '2021-03-22 14:25:16', '2021-03-22 22:42:57', NULL);
INSERT INTO `sys_admin_authmenus` VALUES (3, 2, '添加', '', '#', 'sys-menus-add', b'0', 9, b'0', 1, '2021-03-22 14:25:16', NULL, NULL);
INSERT INTO `sys_admin_authmenus` VALUES (4, 2, '修改', '', '#', 'sys-menus-edit', b'0', 7, b'0', 1, '2021-03-22 14:25:16', NULL, NULL);
INSERT INTO `sys_admin_authmenus` VALUES (5, 2, '删除', '', '#', 'sys-menus-delete', b'0', 8, b'0', 1, '2021-03-22 14:25:16', NULL, NULL);
INSERT INTO `sys_admin_authmenus` VALUES (16, 1, '角色管理', 'icon-lock text-danger', '/admin/sys/admin/role/list', 'sys-role', b'1', 98, b'0', 1, '2021-03-22 14:25:16', NULL, NULL);
INSERT INTO `sys_admin_authmenus` VALUES (18, 16, '添加', '', '#', 'sys-role-add', b'0', 9, b'0', 1, '2021-03-22 14:25:16', NULL, NULL);
INSERT INTO `sys_admin_authmenus` VALUES (19, 16, '修改', '', '#', 'sys-role-edit', b'0', 7, b'0', 1, '2021-03-22 14:25:16', NULL, NULL);
INSERT INTO `sys_admin_authmenus` VALUES (20, 16, '删除', '', '#', 'sys-role-delete', b'0', 8, b'0', 1, '2021-03-22 14:25:16', NULL, NULL);
INSERT INTO `sys_admin_authmenus` VALUES (21, 1, '帐号管理', 'icon-users text-success', '/admin/sys/admin/user/list', 'sys-admin', b'1', 97, b'0', 1, '2021-03-22 14:25:16', NULL, NULL);
INSERT INTO `sys_admin_authmenus` VALUES (22, 21, '添加', '', '#', 'sys-admin-add', b'0', 9, b'0', 1, '2021-03-22 14:25:16', NULL, NULL);
INSERT INTO `sys_admin_authmenus` VALUES (26, 21, '禁止登录', '', '#', 'sys-admin-login', b'0', 5, b'0', 1, '2021-03-22 14:25:16', NULL, NULL);
INSERT INTO `sys_admin_authmenus` VALUES (28, 21, '授权', '', '#', 'sys-admin-accredit', b'0', 4, b'0', 1, '2021-03-22 14:25:16', NULL, NULL);
INSERT INTO `sys_admin_authmenus` VALUES (29, 1, '登录记录', 'fa fa-eye text-dark', '/admin/sys/admin/record', 'sys-loginrecord', b'1', 96, b'0', 1, '2021-03-22 14:25:16', NULL, NULL);
INSERT INTO `sys_admin_authmenus` VALUES (183, 181, '修改', '', '#', 'travel-strategy-edit', b'0', 0, b'0', 1, '2021-03-22 14:25:16', NULL, NULL);
INSERT INTO `sys_admin_authmenus` VALUES (304, 21, '修改', '', '#', 'sys-admin-edit', b'0', 7, b'0', 1, '2021-03-22 14:25:16', NULL, NULL);
INSERT INTO `sys_admin_authmenus` VALUES (339, 21, '删除', '', '#', 'sys-admin-delete', b'0', 8, b'0', 1, '2021-03-22 14:25:16', NULL, NULL);
INSERT INTO `sys_admin_authmenus` VALUES (344, 1, '公告管理', 'fa fa-bullhorn text-success', '/admin/sys/admin/notices/list', 'sys-notice', b'1', 95, b'0', 1, '2021-03-22 14:25:16', NULL, NULL);
INSERT INTO `sys_admin_authmenus` VALUES (345, 344, '添加', '', '#', 'sys-notice-add', b'0', 9, b'0', 1, '2021-03-22 14:25:16', NULL, NULL);
INSERT INTO `sys_admin_authmenus` VALUES (346, 344, '修改', '', '#', 'sys-notice-edit', b'0', 7, b'0', 1, '2021-03-22 14:25:16', NULL, NULL);
INSERT INTO `sys_admin_authmenus` VALUES (347, 344, '删除', '', '#', 'sys-notice-delete', b'0', 8, b'0', 1, '2021-03-22 14:25:16', NULL, NULL);
INSERT INTO `sys_admin_authmenus` VALUES (348, 0, '内容管理', 'fa fa-book text-success', '#', 'content', b'1', 0, b'0', 1, '2021-03-22 14:25:16', '2021-03-30 13:53:30', NULL);
INSERT INTO `sys_admin_authmenus` VALUES (357, 348, '文章管理', 'fa fa-folder-open-o text-warning', '/admin/articles/list', 'content-article', b'1', 97, b'1', 1, '2021-03-22 14:25:16', NULL, NULL);
INSERT INTO `sys_admin_authmenus` VALUES (358, 357, '添加', '', '#', 'content-article-add', b'0', 9, b'0', 1, '2021-03-22 14:25:16', NULL, NULL);
INSERT INTO `sys_admin_authmenus` VALUES (359, 357, '修改', '', '#', 'content-article-edit', b'0', 7, b'0', 1, '2021-03-22 14:25:16', NULL, NULL);
INSERT INTO `sys_admin_authmenus` VALUES (360, 357, '删除', '', '#', 'content-article-delete', b'0', 8, b'0', 1, '2021-03-22 14:25:16', NULL, NULL);
INSERT INTO `sys_admin_authmenus` VALUES (361, 348, '友链审核', 'icon-link', '/admin/link/list', 'content-link', b'1', 0, b'1', 1, '2021-03-22 14:25:16', NULL, NULL);
INSERT INTO `sys_admin_authmenus` VALUES (362, 361, '删除', '', '#', 'content-link-delete', b'0', 8, b'0', 1, '2021-03-22 14:25:16', NULL, NULL);
INSERT INTO `sys_admin_authmenus` VALUES (363, 361, '修改', '', '#', 'content-link-edit', b'0', 7, b'0', 1, '2021-03-22 14:25:16', NULL, NULL);
INSERT INTO `sys_admin_authmenus` VALUES (364, 1, '客户管理', 'icon-users text-primary', '/admin/sys/admin/customer', 'sys-customer', b'1', 0, b'0', 1, '2021-03-22 14:25:16', NULL, NULL);
INSERT INTO `sys_admin_authmenus` VALUES (365, 364, '修改', '', '#', 'sys-customer-edit', b'0', 7, b'0', 1, '2021-03-22 14:25:16', NULL, NULL);
INSERT INTO `sys_admin_authmenus` VALUES (366, 364, '删除', '', '#', 'sys-customer-delete', b'0', 8, b'0', 1, '2021-03-22 14:25:16', NULL, NULL);
INSERT INTO `sys_admin_authmenus` VALUES (367, 364, '手机', '', '#', 'sys-customer-mobile', b'0', 5, b'0', 1, '2021-03-22 14:25:16', NULL, NULL);
INSERT INTO `sys_admin_authmenus` VALUES (368, 2, '列表', '', '#', 'sys-menus-list', b'0', 6, b'0', 1, '2021-03-22 14:25:16', NULL, NULL);
INSERT INTO `sys_admin_authmenus` VALUES (369, 16, '列表', '', '#', 'sys-role-list', b'0', 6, b'0', 1, '2021-03-22 14:25:16', NULL, NULL);
INSERT INTO `sys_admin_authmenus` VALUES (370, 344, '列表', '', '#', 'sys-notice-list', b'0', 6, b'0', 1, '2021-03-22 14:25:16', NULL, NULL);
INSERT INTO `sys_admin_authmenus` VALUES (371, 21, '列表', '', '#', 'sys-admin-list', b'0', 6, b'0', 1, '2021-03-22 14:25:16', NULL, NULL);
INSERT INTO `sys_admin_authmenus` VALUES (372, 364, '列表', '', '#', 'sys-customer-list', b'0', 6, b'0', 1, '2021-03-22 14:25:16', NULL, NULL);
INSERT INTO `sys_admin_authmenus` VALUES (375, 357, '列表', '', '#', 'content-article-list', b'0', 6, b'0', 1, '2021-03-22 14:25:16', NULL, NULL);
INSERT INTO `sys_admin_authmenus` VALUES (376, 361, '列表', '', '#', 'content-link-list', b'0', 6, b'0', 1, '2021-03-22 14:25:16', NULL, NULL);
INSERT INTO `sys_admin_authmenus` VALUES (378, 393, '类别管理', 'icon-social-dropbox text-danger', '/admin/sys/base/category/list', 'base-category', b'1', 0, b'0', 1, '2021-03-22 14:25:16', NULL, NULL);
INSERT INTO `sys_admin_authmenus` VALUES (379, 378, '添加', '', '#', 'base-category-add', b'0', 0, b'0', 1, '2021-03-22 14:25:16', NULL, NULL);
INSERT INTO `sys_admin_authmenus` VALUES (380, 378, '修改', '', '#', 'base-category-edit', b'0', 0, b'0', 1, '2021-03-22 14:25:16', NULL, NULL);
INSERT INTO `sys_admin_authmenus` VALUES (381, 378, '删除', '', '#', 'base-category-delete', b'0', 0, b'0', 1, '2021-03-22 14:25:16', NULL, NULL);
INSERT INTO `sys_admin_authmenus` VALUES (382, 378, '列表', '', '#', 'base-category-list', b'0', 0, b'0', 1, '2021-03-22 14:25:16', NULL, NULL);
INSERT INTO `sys_admin_authmenus` VALUES (383, 393, '枚举管理', 'icon-social-dropbox text-danger', '/admin/sys/base/enums/list', 'base-enum', b'1', 0, b'0', 1, '2021-03-22 14:25:16', NULL, NULL);
INSERT INTO `sys_admin_authmenus` VALUES (384, 383, '添加', '', '#', 'base-enum-add', b'0', 0, b'0', 1, '2021-03-22 14:25:16', NULL, NULL);
INSERT INTO `sys_admin_authmenus` VALUES (385, 383, '修改', '', '#', 'base-enum-edit', b'0', 0, b'0', 1, '2021-03-22 14:25:16', NULL, NULL);
INSERT INTO `sys_admin_authmenus` VALUES (386, 383, '删除', '', '#', 'base-enum-delete', b'0', 0, b'0', 1, '2021-03-22 14:25:16', NULL, NULL);
INSERT INTO `sys_admin_authmenus` VALUES (387, 383, '列表', '', '#', 'base-enum-list', b'0', 0, b'0', 1, '2021-03-22 14:25:16', NULL, NULL);
INSERT INTO `sys_admin_authmenus` VALUES (388, 393, '配置管理', 'icon-social-dropbox text-danger', '/admin/sys/base/config/list', 'base-config', b'1', 0, b'0', 1, '2021-03-22 14:25:16', NULL, NULL);
INSERT INTO `sys_admin_authmenus` VALUES (389, 388, '添加', '', '#', 'base-config-add', b'0', 0, b'0', 1, '2021-03-22 14:25:16', NULL, NULL);
INSERT INTO `sys_admin_authmenus` VALUES (390, 388, '修改', '', '#', 'base-config-edit', b'0', 0, b'0', 1, '2021-03-22 14:25:16', NULL, NULL);
INSERT INTO `sys_admin_authmenus` VALUES (391, 388, '删除', '', '#', 'base-config-delete', b'0', 0, b'0', 1, '2021-03-22 14:25:16', NULL, NULL);
INSERT INTO `sys_admin_authmenus` VALUES (392, 388, '列表', '', '#', 'base-config-list', b'0', 0, b'0', 1, '2021-03-22 14:25:16', NULL, NULL);
INSERT INTO `sys_admin_authmenus` VALUES (393, 0, '公共配置', 'icon-paper-clip text-primary', '#', 'base', b'1', 98, b'0', 1, '2021-03-22 14:25:16', NULL, NULL);
INSERT INTO `sys_admin_authmenus` VALUES (394, 0, '我的简历', 'icon-users', '/admin/resume/list', 'content-resume', b'1', 0, b'0', 1, '2021-03-22 14:25:16', NULL, NULL);
INSERT INTO `sys_admin_authmenus` VALUES (395, 394, '基本信息-添加', '', '#', 'content-resume-add', b'0', 0, b'0', 1, '2021-03-22 14:25:16', NULL, NULL);
INSERT INTO `sys_admin_authmenus` VALUES (396, 394, '基本信息-删除', '', '#', 'content-resume-delete', b'0', 0, b'0', 1, '2021-03-22 14:25:16', NULL, NULL);
INSERT INTO `sys_admin_authmenus` VALUES (397, 394, '基本信息-修改', '', '#', 'content-resume-edit', b'0', 0, b'0', 1, '2021-03-22 14:25:16', NULL, NULL);
INSERT INTO `sys_admin_authmenus` VALUES (398, 394, '基本信息-列表', '', '#', 'content-resume-list', b'0', 0, b'0', 1, '2021-03-22 14:25:16', NULL, NULL);
INSERT INTO `sys_admin_authmenus` VALUES (399, 394, '求职意向', '', '#', 'content-resume-intention', b'0', 0, b'0', 1, '2021-03-22 14:25:16', NULL, NULL);
INSERT INTO `sys_admin_authmenus` VALUES (401, 394, '工作经验-列表', '', '#', 'content-resume-work-list', b'0', 0, b'0', 1, '2021-03-22 14:25:16', NULL, NULL);
INSERT INTO `sys_admin_authmenus` VALUES (402, 394, '工作经验-添加', '', '#', 'content-resume-work-add', b'0', 0, b'0', 1, '2021-03-22 14:25:16', NULL, NULL);
INSERT INTO `sys_admin_authmenus` VALUES (403, 394, '工作经验-修改', '', '#', 'content-resume-work-edit', b'0', 0, b'0', 1, '2021-03-22 14:25:16', NULL, NULL);
INSERT INTO `sys_admin_authmenus` VALUES (404, 394, '工作经验-删除', '', '#', 'content-resume-work-delete', b'0', 0, b'0', 1, '2021-03-22 14:25:16', NULL, NULL);
INSERT INTO `sys_admin_authmenus` VALUES (405, 394, '项目经验-列表', '', '#', 'content-resume-project-list', b'0', 0, b'0', 1, '2021-03-22 14:25:16', NULL, NULL);
INSERT INTO `sys_admin_authmenus` VALUES (406, 394, '工作经验-添加', '', '#', 'content-resume-project-add', b'0', 0, b'0', 1, '2021-03-22 14:25:16', NULL, NULL);
INSERT INTO `sys_admin_authmenus` VALUES (407, 394, '工作经验-修改', '', '#', 'content-resume-project-edit', b'0', 0, b'0', 1, '2021-03-22 14:25:16', NULL, NULL);
INSERT INTO `sys_admin_authmenus` VALUES (408, 394, '工作经验-删除', '', '#', 'content-resume-project-delete', b'0', 0, b'0', 1, '2021-03-22 14:25:16', NULL, NULL);
INSERT INTO `sys_admin_authmenus` VALUES (409, 394, '获奖证书-列表', '', '#', 'content-resume-credential-list', b'0', 0, b'0', 1, '2021-03-22 14:25:16', NULL, NULL);
INSERT INTO `sys_admin_authmenus` VALUES (410, 394, '获奖证书-添加', '', '#', 'content-resume-credential-add', b'0', 0, b'0', 1, '2021-03-22 14:25:16', NULL, NULL);
INSERT INTO `sys_admin_authmenus` VALUES (411, 394, '获奖证书-修改', '', '#', 'content-resume-credential-edit', b'0', 0, b'0', 1, '2021-03-22 14:25:16', NULL, NULL);
INSERT INTO `sys_admin_authmenus` VALUES (412, 394, '获奖证书-删除', '', '#', 'content-resume-credential-delete', b'0', 0, b'0', 1, '2021-03-22 14:25:16', NULL, NULL);
INSERT INTO `sys_admin_authmenus` VALUES (413, 394, '教育经历-列表', '', '#', 'content-resume-education-list', b'0', 0, b'0', 1, '2021-03-22 14:25:16', NULL, NULL);
INSERT INTO `sys_admin_authmenus` VALUES (414, 394, '教育经历-添加', '', '#', 'content-resume-education-add', b'0', 0, b'0', 1, '2021-03-22 14:25:16', NULL, NULL);
INSERT INTO `sys_admin_authmenus` VALUES (415, 394, '教育经历-修改', '', '#', 'content-resume-education-edit', b'0', 0, b'0', 1, '2021-03-22 14:25:16', NULL, NULL);
INSERT INTO `sys_admin_authmenus` VALUES (416, 394, '教育经历-删除', '', '#', 'content-resume-education-delete', b'0', 0, b'0', 1, '2021-03-22 14:25:16', NULL, NULL);
INSERT INTO `sys_admin_authmenus` VALUES (417, 394, '我的作品-列表', '', '#', 'content-resume-sample-list', b'0', 0, b'0', 1, '2021-03-22 14:25:16', NULL, NULL);
INSERT INTO `sys_admin_authmenus` VALUES (418, 394, '我的作品-添加', '', '#', 'content-resume-sample-add', b'0', 0, b'0', 1, '2021-03-22 14:25:16', NULL, NULL);
INSERT INTO `sys_admin_authmenus` VALUES (419, 394, '我的作品-修改', '', '#', 'content-resume-sample-edit', b'0', 0, b'0', 1, '2021-03-22 14:25:16', NULL, NULL);
INSERT INTO `sys_admin_authmenus` VALUES (420, 394, '我的作品-删除', '', '#', 'content-resume-sample-delete', b'0', 0, b'0', 1, '2021-03-22 14:25:16', NULL, NULL);
INSERT INTO `sys_admin_authmenus` VALUES (421, 394, '拷贝简历', '', '#', 'content-resume-copy', b'0', 0, b'0', 1, '2021-03-22 14:25:16', NULL, NULL);
INSERT INTO `sys_admin_authmenus` VALUES (423, 393, '国家地区', 'icon-social-dribbble', '/admin/sys/admin/chinaarea/list', 'sys-chinaarea', b'1', 0, b'0', 1, '2021-04-27 14:19:17', NULL, NULL);
INSERT INTO `sys_admin_authmenus` VALUES (424, 423, '添加', '', '#', 'sys-chinaarea-add', b'0', 0, b'0', 1, '2021-04-27 14:19:59', NULL, NULL);
INSERT INTO `sys_admin_authmenus` VALUES (425, 423, '修改', '', '#', 'sys-chinaarea-edit', b'0', 0, b'0', 1, '2021-04-27 14:20:10', NULL, NULL);
INSERT INTO `sys_admin_authmenus` VALUES (426, 423, '删除', '', '#', 'sys-chinaarea-delete', b'0', 0, b'0', 1, '2021-04-27 14:20:21', NULL, NULL);
INSERT INTO `sys_admin_authmenus` VALUES (427, 423, '列表', '', '#', 'sys-chinaarea-list', b'0', 0, b'0', 1, '2021-04-27 14:20:29', NULL, NULL);


-- ----------------------------
-- Records of sys_admin_authrole
-- ----------------------------
INSERT INTO `sys_admin_authrole` VALUES (1, '超级管理员', 1, '2021-03-22 14:25:20', '2021-05-07 21:35:25', NULL);


-- ----------------------------
-- Records of sys_admin_authrolemenus
-- ----------------------------
INSERT INTO `sys_admin_authrolemenus` VALUES (21022, 1, 398);
INSERT INTO `sys_admin_authrolemenus` VALUES (21023, 1, 397);
INSERT INTO `sys_admin_authrolemenus` VALUES (21024, 1, 396);
INSERT INTO `sys_admin_authrolemenus` VALUES (21025, 1, 395);
INSERT INTO `sys_admin_authrolemenus` VALUES (21026, 1, 394);
INSERT INTO `sys_admin_authrolemenus` VALUES (21027, 1, 376);
INSERT INTO `sys_admin_authrolemenus` VALUES (21028, 1, 363);
INSERT INTO `sys_admin_authrolemenus` VALUES (21029, 1, 362);
INSERT INTO `sys_admin_authrolemenus` VALUES (21030, 1, 361);
INSERT INTO `sys_admin_authrolemenus` VALUES (21031, 1, 375);
INSERT INTO `sys_admin_authrolemenus` VALUES (21032, 1, 359);
INSERT INTO `sys_admin_authrolemenus` VALUES (21033, 1, 360);
INSERT INTO `sys_admin_authrolemenus` VALUES (21034, 1, 358);
INSERT INTO `sys_admin_authrolemenus` VALUES (21035, 1, 357);
INSERT INTO `sys_admin_authrolemenus` VALUES (21036, 1, 348);
INSERT INTO `sys_admin_authrolemenus` VALUES (21037, 1, 427);
INSERT INTO `sys_admin_authrolemenus` VALUES (21038, 1, 426);
INSERT INTO `sys_admin_authrolemenus` VALUES (21039, 1, 399);
INSERT INTO `sys_admin_authrolemenus` VALUES (21040, 1, 425);
INSERT INTO `sys_admin_authrolemenus` VALUES (21041, 1, 401);
INSERT INTO `sys_admin_authrolemenus` VALUES (21042, 1, 403);
INSERT INTO `sys_admin_authrolemenus` VALUES (21043, 1, 420);
INSERT INTO `sys_admin_authrolemenus` VALUES (21044, 1, 419);
INSERT INTO `sys_admin_authrolemenus` VALUES (21045, 1, 418);
INSERT INTO `sys_admin_authrolemenus` VALUES (21046, 1, 417);
INSERT INTO `sys_admin_authrolemenus` VALUES (21047, 1, 416);
INSERT INTO `sys_admin_authrolemenus` VALUES (21048, 1, 415);
INSERT INTO `sys_admin_authrolemenus` VALUES (21049, 1, 414);
INSERT INTO `sys_admin_authrolemenus` VALUES (21050, 1, 413);
INSERT INTO `sys_admin_authrolemenus` VALUES (21051, 1, 412);
INSERT INTO `sys_admin_authrolemenus` VALUES (21052, 1, 411);
INSERT INTO `sys_admin_authrolemenus` VALUES (21053, 1, 410);
INSERT INTO `sys_admin_authrolemenus` VALUES (21054, 1, 409);
INSERT INTO `sys_admin_authrolemenus` VALUES (21055, 1, 408);
INSERT INTO `sys_admin_authrolemenus` VALUES (21056, 1, 407);
INSERT INTO `sys_admin_authrolemenus` VALUES (21057, 1, 406);
INSERT INTO `sys_admin_authrolemenus` VALUES (21058, 1, 405);
INSERT INTO `sys_admin_authrolemenus` VALUES (21059, 1, 404);
INSERT INTO `sys_admin_authrolemenus` VALUES (21060, 1, 402);
INSERT INTO `sys_admin_authrolemenus` VALUES (21061, 1, 421);
INSERT INTO `sys_admin_authrolemenus` VALUES (21062, 1, 424);
INSERT INTO `sys_admin_authrolemenus` VALUES (21063, 1, 392);
INSERT INTO `sys_admin_authrolemenus` VALUES (21064, 1, 28);
INSERT INTO `sys_admin_authrolemenus` VALUES (21065, 1, 26);
INSERT INTO `sys_admin_authrolemenus` VALUES (21066, 1, 371);
INSERT INTO `sys_admin_authrolemenus` VALUES (21067, 1, 304);
INSERT INTO `sys_admin_authrolemenus` VALUES (21068, 1, 339);
INSERT INTO `sys_admin_authrolemenus` VALUES (21069, 1, 22);
INSERT INTO `sys_admin_authrolemenus` VALUES (21070, 1, 21);
INSERT INTO `sys_admin_authrolemenus` VALUES (21071, 1, 369);
INSERT INTO `sys_admin_authrolemenus` VALUES (21072, 1, 19);
INSERT INTO `sys_admin_authrolemenus` VALUES (21073, 1, 20);
INSERT INTO `sys_admin_authrolemenus` VALUES (21074, 1, 18);
INSERT INTO `sys_admin_authrolemenus` VALUES (21075, 1, 16);
INSERT INTO `sys_admin_authrolemenus` VALUES (21076, 1, 368);
INSERT INTO `sys_admin_authrolemenus` VALUES (21077, 1, 4);
INSERT INTO `sys_admin_authrolemenus` VALUES (21078, 1, 5);
INSERT INTO `sys_admin_authrolemenus` VALUES (21079, 1, 3);
INSERT INTO `sys_admin_authrolemenus` VALUES (21080, 1, 2);
INSERT INTO `sys_admin_authrolemenus` VALUES (21081, 1, 29);
INSERT INTO `sys_admin_authrolemenus` VALUES (21082, 1, 423);
INSERT INTO `sys_admin_authrolemenus` VALUES (21083, 1, 344);
INSERT INTO `sys_admin_authrolemenus` VALUES (21084, 1, 347);
INSERT INTO `sys_admin_authrolemenus` VALUES (21085, 1, 391);
INSERT INTO `sys_admin_authrolemenus` VALUES (21086, 1, 390);
INSERT INTO `sys_admin_authrolemenus` VALUES (21087, 1, 389);
INSERT INTO `sys_admin_authrolemenus` VALUES (21088, 1, 388);
INSERT INTO `sys_admin_authrolemenus` VALUES (21089, 1, 387);
INSERT INTO `sys_admin_authrolemenus` VALUES (21090, 1, 386);
INSERT INTO `sys_admin_authrolemenus` VALUES (21091, 1, 385);
INSERT INTO `sys_admin_authrolemenus` VALUES (21092, 1, 384);
INSERT INTO `sys_admin_authrolemenus` VALUES (21093, 1, 383);
INSERT INTO `sys_admin_authrolemenus` VALUES (21094, 1, 382);
INSERT INTO `sys_admin_authrolemenus` VALUES (21095, 1, 381);
INSERT INTO `sys_admin_authrolemenus` VALUES (21096, 1, 380);
INSERT INTO `sys_admin_authrolemenus` VALUES (21097, 1, 379);
INSERT INTO `sys_admin_authrolemenus` VALUES (21098, 1, 378);
INSERT INTO `sys_admin_authrolemenus` VALUES (21099, 1, 393);
INSERT INTO `sys_admin_authrolemenus` VALUES (21100, 1, 370);
INSERT INTO `sys_admin_authrolemenus` VALUES (21101, 1, 346);
INSERT INTO `sys_admin_authrolemenus` VALUES (21102, 1, 345);
INSERT INTO `sys_admin_authrolemenus` VALUES (21103, 1, 1);


-- ----------------------------
-- Records of sys_admin_users
-- ----------------------------
INSERT INTO `sys_admin_users` VALUES (22, 'admin', '13500000000', '21232F297A57A5A743894A0E4A801FC3', 'Nigel', '/upload/image/20200804/761a8e29f77d4859865c6b889668c8ed.jpg', '长沙', '架构师', '保密', 2041, '2021-06-03 11:38:34', '113.247.128.111', 1, '2021-03-22 14:25:32', '2021-04-14 10:40:11', NULL);

-- ----------------------------
-- Records of sys_admin_authuserrole
-- ----------------------------
INSERT INTO `sys_admin_authuserrole` VALUES (339, 22, 1);