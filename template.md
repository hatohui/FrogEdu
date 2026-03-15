As #file:backend-spec.md

use #shadcn and #io.github.upstash/context7 to help me implement the flow where:

- analysis thì phải có thể chọn trong vòng tuần, tháng, năm, etc và biểu đồ phải update theo đó
- Cái gói pro chưa có match với cái feature của mình.
- không có analytic AI usage, không có lưu lại là những user nào đã xài bao nhiêu trong khoảng thời gian nào đó cho admin và người dùng cũng không xem được usage của mình.
- Cần bổ sung thêm API để lấy thống kê usage của AI cho người dùng và admin, có thể theo ngày, tuần, tháng.
- Cần bổ sung thêm UI để hiển thị thống kê usage của AI cho người dùng và admin trong dashboard analytics, có thể là một tab riêng hoặc một phần trong tab overview.
- Need to have usage statistics for AI features, including number of times used, time spent, and user breakdown. This will help us understand how users are engaging with AI features and identify areas for improvement.
  Let users see it in their dashboard so they can track their own usage and set goals for themselves. Admins can use this data to monitor overall usage trends and make informed decisions about future AI feature development.
- Cần chạy EF migrations để cập nhật database nếu có thay đổi về schema cho phần thống kê usage

Rules to follow:

- Strictly follows CLEAN architecture for backend
- Frontend should always defines types first in #file:types : models are domain models, the dtos are in dtos, always use hooks and service pattern and use axios + tanstack query for it.
- The responsibility of each microservice should only be in their #file:microservices-details.md
- The page should be globalized by the keys in #language

Help me implement the dashboard and API first, then proceeds to implement the pages and apis, run EF migrations and update if needed
